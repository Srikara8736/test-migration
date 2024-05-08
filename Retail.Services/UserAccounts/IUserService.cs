using Retail.DTOs;
using Retail.DTOs.UserAccounts;
using Retail.Services.Common;

namespace Retail.Services.UserAccounts;

public interface IUserService
{

    #region User

    /// <summary>
    /// gets the User details by User
    /// </summary>
    /// <param name="userDto">userDto</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns> User Infromation</returns>
    Task<ResultDto<UserResponseDto>> GetUserAuthByUser(UserDto userDto, CancellationToken ct = default);


    /// <summary>
    /// gets the User details by Id
    /// </summary>
    /// <param name="id">User Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns> User Infromation</returns>
    Task<ResultDto<UserResponseDto>> GetUserById(Guid id, CancellationToken ct);

    /// <summary>
    /// sets the User details 
    /// </summary>
    /// <param name="userRequestDto">User Request DTO</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>User Information
    Task<ResultDto<UserResponseDto>> RegisterUser(UserDto userRequestDto, CancellationToken ct);


    /// <summary>
    /// gets all Users
    /// </summary>
    /// <param name="pageIndex">page Indes</param>
    /// <param name="pageSize">page size</param>
    /// <param name="ct">cancellation token</param>
    /// <returns></returns>
    Task<PaginationResultDto<PagedList<UserResponseDto>>> GetAllUsers(Guid? customerId = null, string? keyword = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default);

    /// <summary>
    /// Update Users
    /// </summary>
    /// <param name="id"> Id</param>
    /// <param name="userDto">Users</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Response User</returns>
    Task<ResultDto<UserResponseDto>> UpdateUser(Guid id, UserDto userDto, CancellationToken ct = default);


    /// <summary>
    /// Update User Password
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Response User Password updated status</returns>
    Task<ResultDto<bool>> UpdateUserPassword(Guid id, string password, CancellationToken ct = default);


    /// <summary>
    /// ChangeUserPassword
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Response User Password updated status</returns>
    Task<ResultDto<bool>> ChangeUserPassword(Guid id, PasswordDto password, CancellationToken ct = default);

    /// <summary>
    /// Delete Users
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Response User</returns>
    Task<ResultDto<UserResponseDto>> DeleteUser(Guid id, CancellationToken ct = default);

    #endregion

    
}

