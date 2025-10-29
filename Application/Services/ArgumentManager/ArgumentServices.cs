using Application.Entities;
using Application.Exceptions;
using Application.Repositories;
using Application.Services.ArgumentManager.Requests;
using Application.Services.NotificationTemplateManager.Responses;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Application.Services.ArgumentManager;

public class ArgumentServices
{
    private readonly ILogger<ArgumentServices> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbConnection _dbConnection;

    public ArgumentServices(ILogger<ArgumentServices> logger,IUnitOfWork unitOfWork, IDbConnection dbConnection)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _dbConnection = dbConnection;
    }
    /// <summary>
    ///  Create new instance for arguments
    /// </summary>
    /// <param name="request">information need created argument</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return argument response after create success
    /// </returns>

    public async Task<ArgumentResponse> CreateArgumentAsync(CreateArgumentRequest request,
        CancellationToken cancellationToken = default)
    {
        Arguments? arguments = await _unitOfWork
            .ArgumentRepository.GetByCodeAsync(request.Code, cancellationToken);
        ThrowHelper.ThrowBusinessErrorWhenExitsItem(arguments, ArgumentConstMessage.CodeHasExits);
        arguments = request.MapToArgument();

        // check valid query string is correct format
        /*await ArgumentBusinessRule.CreateRule(arguments)
            .CheckInvalidQueryAsync(_dbContection);*/

        _unitOfWork.ArgumentRepository.Add(arguments);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return arguments.MapToResponse();
    }
    /// <summary>
    ///   Update information with argument
    /// </summary>
    /// <param name="request">information need update argument</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return argument response after updated success
    /// </returns>

    public async Task<ArgumentResponse> UpdateArgumentAsync(UpdateArgumentResponse request,
        CancellationToken cancellationToken = default)
    {
        Arguments? arguments = await _unitOfWork.ArgumentRepository
            .GetByIdAsync(request.ArgumentId, cancellationToken);
        ThrowHelper.ThrowWhenNotFoundItem(arguments, ArgumentConstMessage.ArgumentNotFound);

        Arguments? argumentByCode = await _unitOfWork.ArgumentRepository
            .GetByCodeAsync(request.Code, cancellationToken);
        if (argumentByCode is not null && argumentByCode.Code != arguments.Code)
        {
            ThrowHelper.ThrowWhenBusinessError(ArgumentConstMessage.CodeHasExits);
        }
        request.MapToArgument(arguments);
        // check valid query string is correct format
        /*await ArgumentBusinessRule.CreateRule(arguments)
            .CheckInvalidQueryAsync(_dbContection);*/

        _unitOfWork.ArgumentRepository.Update(arguments);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return arguments.MapToResponse();
    }
}