using Retail.DTOs.Customers;
using Retail.DTOs.Master;
using Retail.DTOs;
using Retail.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.Services.Master;


    public interface ICodeMasterService
    {

    #region Status

    /// <summary>
    /// gets the Status details by Id
    /// </summary>
    /// <param name="id">Status Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns> Status Infromation</returns>
    Task<ResultDto<CodeMasterResponseDto>> GetStatusById(Guid id, Guid? customerId, CancellationToken ct);


        /// <summary>
        /// Creates a new Status
        /// </summary>
        /// <param name="StatusDto">Status</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<ResultDto<CodeMasterResponseDto>> InsertStatus(CodeMasterDto StatusDto, CancellationToken ct = default);

        /// <summary>
        /// Updates the existing Status
        /// </summary>
        /// <param name="id">StatusId</param>
        /// <param name="StatusDto">Status</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<ResultDto<CodeMasterResponseDto>> UpdateStatus(Guid StatusId, CodeMasterDto StatusDto, CancellationToken ct = default);

        /// <summary>
        /// Delete an existing Status
        /// </summary>
        /// <param name="id">StatusID</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns></returns>
        Task<ResultDto<bool>> DeleteStatus(Guid id, CancellationToken ct = default);

        /// <summary>
        /// gets all the Status
        /// </summary>.
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PaginationResultDto<PagedList<CodeMasterResponseDto>>> GetAllStatus(string keyword = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default);


        /// <summary>
        /// gets all the Status
        /// </summary>.
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PaginationResultDto<PagedList<CodeMasterResponseDto>>> GetAllStoreStatus(string keyword = null,Guid? customerId = null, int pageIndex = 0, int pageSize = 0, CancellationToken ct = default);

    #endregion


    #region Customer region


    /// <summary>
    /// gets the Status details by Id
    /// </summary>
    /// <param name="id">Status Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns> Status Infromation</returns>
    Task<ResultDto<CustomerCodeMasterResponseDto>> GetCustomerStatusById(Guid id, CancellationToken ct);

    /// <summary>
    /// Creates a new Status
    /// </summary>
    /// <param name="StatusDto">Status</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<ResultDto<CodeMasterResponseDto>> InsertCustomerStatus(CustomerCodeMasterDto StatusDto, CancellationToken ct = default);

    /// <summary>
    /// Updates the existing Status
    /// </summary>
    /// <param name="id">StatusId</param>
    /// <param name="StatusDto">Status</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<ResultDto<CodeMasterResponseDto>> UpdateCustomerStatus(Guid customerStatusId, CustomerCodeMasterDto StatusDto, CancellationToken ct = default);

    #endregion


}
