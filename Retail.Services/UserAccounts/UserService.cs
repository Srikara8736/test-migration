using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Retail.Data.Entities.UserAccount;
using Retail.Data.Repository;
using Retail.DTOs;
using Retail.DTOs.Customers;
using Retail.DTOs.UserAccounts;
using Retail.Services.Common;
using Retail.Services.Customers;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Retail.Services.UserAccounts;

/// <summary>
/// User Service
/// </summary>
public class UserService : IUserService
{
    #region Fields

    private readonly RepositoryContext _repositoryContext;
    private readonly IMapper _mapper;
    private readonly ICustomerService _customerService;

    #endregion

    #region Ctor
    public UserService(RepositoryContext repositoryContext, IMapper mapper, ICustomerService customerService)
    {
        _repositoryContext = repositoryContext;
        _mapper = mapper;
        _customerService = customerService;
    }
    #endregion

    #region Utitlities

    /// <summary>
    /// encodes the password 
    /// </summary>
    /// <param name="password">password</param>
    /// <returns>Hashed Password</returns>
    public string HashPassword(string password)
    {
        SHA256 hash = SHA256.Create();
        var passwordBytes = Encoding.Default.GetBytes(password);
        var hashedPassword = hash.ComputeHash(passwordBytes);
        return Convert.ToHexString(hashedPassword);
    }



    /// <summary>
    /// Validate User Is Already Exists
    /// </summary>
    /// <param name="emailAdrress">emailAdrress</param>
    /// <returns>Flag Value</returns>
    public bool ValidateUserEmail(string emailAdrress)
    {
        bool isExists = false;

        if (emailAdrress != "" && emailAdrress != null)
            isExists = _repositoryContext.Users.Any(x => x.IsDeleted == false && x.Email.ToLower().Trim() == emailAdrress.ToLower().Trim());

        return isExists;

    }


    public async Task<UserRole>  InsertUserRole(UserRole userRole, CancellationToken ct)
    {
        try
        {
            await _repositoryContext.UserRoles.AddAsync(userRole, ct);
            await _repositoryContext.SaveChangesAsync(ct);
            return userRole;
        }
        catch (Exception ex)
        {
            return null;
        }
        
    }


    public async Task<UserRole> UpdateUserRole(UserRole userRole, CancellationToken ct)
    {
        try
        {
            var userRoleItem = await _repositoryContext.UserRoles.Where(x => x.UserId == userRole.UserId).FirstOrDefaultAsync();
            if (userRoleItem == null)
                return null;

            userRoleItem.RoleId = userRole.RoleId;
            


            await _repositoryContext.SaveChangesAsync(ct);
            return userRoleItem;
        }
        catch (Exception ex)
        {
            return null;
        }

    }



    #endregion


    #region Methods


    #region User

    /// <summary>
    /// gets the user details by UserName
    /// </summary>
    /// <param name="userName">User Name</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>User Details</returns>
    public async Task<ResultDto<UserResponseDto>> GetUserAuthByUser(UserDto userDto, CancellationToken ct = default)
    {
        if (userDto == null)
        {
            var UserResult = new ResultDto<UserResponseDto>
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.BadRequest
            };
            return UserResult;
        }
        var user = await _repositoryContext.Users.FirstOrDefaultAsync(x => x.UserName == userDto.UserName && x.IsDeleted == false, ct);
        if (user == null)
        {
            var UserResult = new ResultDto<UserResponseDto>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return UserResult;
        }

        var userDetails = _mapper.Map<UserDto>(user);

        var password = HashPassword(userDto.PasswordHash);

        if (userDetails.PasswordHash.ToUpper() != password)
        {
            var UserResult = new ResultDto<UserResponseDto>()
            {
                ErrorMessage = StringResources.Unauthorized,
                StatusCode = HttpStatusCode.Unauthorized
            };
            return UserResult;
        }

        var userResponse = _mapper.Map<UserResponseDto>(user);

        var userRole = _repositoryContext.UserRoles.Where(x => x.UserId == userResponse.Id).FirstOrDefault();
        if (userRole != null)
        {
            userResponse.RoleName = (await _repositoryContext.Roles.FirstOrDefaultAsync(x => x.Id == userRole.RoleId, ct))?.Name ?? string.Empty;
            userResponse.RoleId = userRole.RoleId;
        }



        if (userResponse.CustomerId != null)
        {
            //var customer = await _repositoryContext.Customers.FirstOrDefaultAsync(x => x.Id == userResponse.CustomerId, ct);
            var customer = await _customerService.GetCustomerById((Guid)userResponse.CustomerId,ct);
            userResponse.Customer = customer.Data;

        }

        var response = new ResultDto<UserResponseDto>
        {
            IsSuccess = true,
            Data = userResponse
        };

        return response;
    }


    /// <summary>
    /// Gets the user details by User Id
    /// </summary>
    /// <param name="id">User Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>User Details</returns>
    public async Task<ResultDto<UserResponseDto>> GetUserById(Guid id, CancellationToken ct)
    {
        if (id == null)
        {
            var UserResult = new ResultDto<UserResponseDto>
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.BadRequest
            };
            return UserResult;
        }


        var user = await _repositoryContext.Users.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false, ct);

        if (user == null)
        {
            var UserResult = new ResultDto<UserResponseDto>()
            {
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.NotFound
            };

            return UserResult;
        }

        var userResponse = _mapper.Map<UserResponseDto>(user);

        var userRole = _repositoryContext.UserRoles.Where(x => x.UserId == user.Id).FirstOrDefault();
        if (userRole != null)
        {
            userResponse.RoleName = (await _repositoryContext.Roles.FirstOrDefaultAsync(x => x.Id == userRole.RoleId, ct))?.Name ?? string.Empty;
            userResponse.RoleId = userRole.RoleId;

        }



        if (userResponse.CustomerId != null)
        {
            var customer = await _customerService.GetCustomerById((Guid)userResponse.CustomerId, ct);
            userResponse.Customer = customer.Data;

        }

        var response = new ResultDto<UserResponseDto>
        {
            IsSuccess = true,
            Data = userResponse
        };
        return response;
    }


    /// <summary>
    /// Sets the User details 
    /// </summary>
    /// <param name="userRequestDto">User Request DTO</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns> User Information </returns>
    public async Task<ResultDto<UserResponseDto>> RegisterUser(UserDto userRequestDto, CancellationToken ct = default)
    {
        if (userRequestDto == null)
        {
            var UserResult = new ResultDto<UserResponseDto>()
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.BadRequest
            };
            return UserResult;

        }

      
        var resultUserEmail = ValidateUserEmail(userRequestDto.Email);

        if (resultUserEmail)
        {
            var errorResponse = new ResultDto<UserResponseDto>
            {
                ErrorMessage = StringResources.UserRecordExists,
                StatusCode = HttpStatusCode.Conflict

            };
            return errorResponse;
        }


        var user = _mapper.Map<User>(userRequestDto);

        user.CreatedOn = DateTime.UtcNow;
        user.PhoneNumber = userRequestDto.PhoneNumber;
        //hash password
        var hashedPasword = HashPassword(userRequestDto.PasswordHash);
        user.PasswordHash = hashedPasword;
              

        await _repositoryContext.Users.AddAsync(user, ct);
        await _repositoryContext.SaveChangesAsync(ct);

        var userResponse = _mapper.Map<UserResponseDto>(user);


        var userRole = new UserRole()
        {
            UserId = user.Id,
            RoleId  = userRequestDto.RoleId,
        };

        var  userRoleResult = await InsertUserRole(userRole,ct);
      


       // var userRoleItem = _repositoryContext.UserRoles.Where(x => x.UserId == userResponse.Id).FirstOrDefault();
        if (userRoleResult != null)
        {
            userResponse.RoleName = (await _repositoryContext.Roles.FirstOrDefaultAsync(x => x.Id == userRoleResult.RoleId, ct))?.Name ?? string.Empty;
            userResponse.RoleId = userRoleResult.RoleId;

        }

        if (userResponse.CustomerId != null)
        {
            var customer = await _customerService.GetCustomerById((Guid)userResponse.CustomerId, ct);
            userResponse.Customer = customer.Data;

        }


        var resultResponse = new ResultDto<UserResponseDto>
        {
            Data = userResponse,
            IsSuccess = true
        };
        return resultResponse;

    }

    /// <summary>
    /// gets all Users
    /// </summary>
    /// <param name="pageIndex">page Indes</param>
    /// <param name="pageSize">page size</param>
    /// <param name="ct">cancellation token</param>
    /// <returns></returns>
    public async Task<PaginationResultDto<PagedList<UserResponseDto>>> GetAllUsers(Guid? customerId = null, string? keyword = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default)
    {
        var query = from p in _repositoryContext.Users
                    where p.IsDeleted == false
                    select p;

        if (customerId != null)
        {
            query = query.Where(x => x.CustomerId == customerId);
        }

        if (keyword != null)
        {
            query = query.Where(x =>  x.Email.Contains(keyword));
        }

        var users = await PagedList<User>.ToPagedList(query.OrderBy(on => on.FirstName), pageIndex, pageSize);

        if (users == null)
        {
            var errorResponse = new PaginationResultDto<PagedList<UserResponseDto>>
            {
                IsSuccess = false,
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.NoContent
            };
        }

        var usersResponse = _mapper.Map<PagedList<UserResponseDto>>(users);

        foreach (var user in usersResponse)
        {
            var userRole = _repositoryContext.UserRoles.Where( x => x.UserId == user.Id).FirstOrDefault();
            if(userRole != null)
            {
                user.RoleName = (await _repositoryContext.Roles.FirstOrDefaultAsync(x => x.Id == userRole.RoleId, ct))?.Name ?? string.Empty;
                user.RoleId = userRole.RoleId;
            }

            if (user.CustomerId != null)
            {
                var customer = await _customerService.GetCustomerById((Guid)user.CustomerId, ct);
                user.Customer = customer.Data;

            }
        }

        var response = new PaginationResultDto<PagedList<UserResponseDto>>
        {
            IsSuccess = true,
            Data = usersResponse,
            TotalCount = users.TotalCount,
            TotalPages = users.TotalPages
        };
        return response;

    }


    /// <summary>
    /// Update User
    /// </summary>
    /// <param name="id">User Id</param>
    /// <param name="user">User</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Response User</returns>
    public virtual async Task<ResultDto<UserResponseDto>> UpdateUser(Guid id, UserDto userDto, CancellationToken ct = default)
    {
        if (userDto == null)
        {
            var response = new ResultDto<UserResponseDto>
            {
                ErrorMessage = StringResources.NoResultsFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return response;

        }

        var result = await _repositoryContext.Users.FirstOrDefaultAsync(x => x.Id == id, ct);

        if (result == null)
        {
            var response = new ResultDto<UserResponseDto>
            {
                ErrorMessage = StringResources.RecordNotFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return response;

        }



        result.FirstName = userDto.FirstName;
        result.LastName = userDto.LastName;
        result.Email = userDto.Email;
        result.PhoneNumber = userDto.PhoneNumber;
        result.UpdatedOn = DateTime.UtcNow;
        result.CustomerId = (Guid)userDto.CustomerId;
        result.StatusId = userDto.StatusId;

        await _repositoryContext.SaveChangesAsync(ct);

        var userRoleUpdate = new UserRole()
        {
            UserId = id,
            RoleId = userDto.RoleId
        };

        var userRoleItem = await UpdateUserRole(userRoleUpdate, ct);

        var userResponse = _mapper.Map<UserResponseDto>(result);



       // var userRole = _repositoryContext.UserRoles.Where(x => x.UserId == userResponse.Id).FirstOrDefault();
        if (userRoleItem != null)
        {
            userResponse.RoleName = (await _repositoryContext.Roles.FirstOrDefaultAsync(x => x.Id == userRoleItem.RoleId, ct))?.Name ?? string.Empty;
            userResponse.RoleId = userRoleItem.RoleId;

        }

        if (userResponse.CustomerId != null)
        {
            var customer = await _customerService.GetCustomerById((Guid)userResponse.CustomerId, ct);
            userResponse.Customer = customer.Data;

        }

        var successResponse = new ResultDto<UserResponseDto>
        {
            IsSuccess = true,
            Data = userResponse,
            StatusCode = HttpStatusCode.OK
        };
        return successResponse;
    }



    /// <summary>
    /// Update User Password
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Response User Password updated status</returns>
    public virtual async Task<ResultDto<bool>> UpdateUserPassword(Guid id, string password, CancellationToken ct = default)
    {

        if (password == null)
        {
            var response = new ResultDto<bool>
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.NotFound
            };
            return response;
        }

        var client = await _repositoryContext.Users.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (client == null)
        {
            var response = new ResultDto<bool>
            {
                ErrorMessage = StringResources.RecordNotFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return response;

        }

        client.PasswordHash = HashPassword(password);
        await _repositoryContext.SaveChangesAsync(ct);

        var successResponse = new ResultDto<bool>
        {
            Data= true,
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK
        };
        return successResponse;



    }




    /// <summary>
    /// ChangeUserPassword
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Response User Password updated status</returns>
    public virtual async Task<ResultDto<bool>> ChangeUserPassword(Guid id, PasswordDto password, CancellationToken ct = default)
    {

        if (password == null)
        {
            var response = new ResultDto<bool>
            {
                ErrorMessage = StringResources.InvalidArgument,
                StatusCode = HttpStatusCode.NotFound
                
            };
            return response;
        }

        var client = await _repositoryContext.Users.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (client == null)
        {
            var response = new ResultDto<bool>
            {
                ErrorMessage = StringResources.RecordNotFound,
                IsSuccess = false
            };
            return response;

        }

        var oldPassword = HashPassword(password.CurrentPassword);
      
        if(client.PasswordHash != oldPassword)
        {
            var response = new ResultDto<bool>
            {
                ErrorMessage = StringResources.WrongPassword,
                IsSuccess= false
            };
            return response;
        }


        client.PasswordHash = HashPassword(password.NewPassword);
        await _repositoryContext.SaveChangesAsync(ct);

        var successResponse = new ResultDto<bool>
        {
            Data= true,
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK
        };
        return successResponse;



    }



    /// <summary>
    /// Delete Users
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Response User</returns>
    public virtual async Task<ResultDto<UserResponseDto>> DeleteUser(Guid id, CancellationToken ct = default)
    {
        var client = await _repositoryContext.Users.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (client == null)
        {
            var response = new ResultDto<UserResponseDto>
            {
                ErrorMessage = StringResources.RecordNotFound,
                StatusCode = HttpStatusCode.NotFound
            };
            return response;

        }

        client.IsDeleted = true;
        await _repositoryContext.SaveChangesAsync(ct);

        var successResponse = new ResultDto<UserResponseDto>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK
        };
        return successResponse;



    }

    #endregion


    #endregion
}

