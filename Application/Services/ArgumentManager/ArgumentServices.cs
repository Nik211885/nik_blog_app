using Application.Adapters;
using Application.Entities;
using Application.Exceptions;
using Application.Extensions;
using Application.Repositories;
using Application.Services.ArgumentManager.Requests;
using Application.Services.NotificationTemplateManager.Responses;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Dynamic;

namespace Application.Services.ArgumentManager;

public class ArgumentServices
{
    private readonly ILogger<ArgumentServices> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConnectionQueryService _connectionQueryServices;

    public ArgumentServices(ILogger<ArgumentServices> logger, IUnitOfWork unitOfWork, IConnectionQueryService connectionQueryService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _connectionQueryServices = connectionQueryService;
    }
    /// <summary>
    ///  Create new instance for arguments
    /// </summary>
    /// <param name="request">information need created argument</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return argument response after create success
    /// </returns>

    public async Task<Arguments> CreateArgumentAsync(CreateArgumentRequest request,
        CancellationToken cancellationToken = default)
    {
        Arguments? arguments = await _unitOfWork
            .ArgumentRepository.GetByCodeAsync(request.Code, cancellationToken);
        ThrowHelper.ThrowBusinessErrorWhenExitsItem(arguments, ArgumentConstMessage.CodeHasExits);
        arguments = request.MapToArgument();

        // check valid query string is correct format
        /*await ArgumentBusinessRule.CreateRule(arguments)
            .CheckInvalidQueryAsync(_dbContection);*/
        await TestArgumentWithQueryAsync(arguments);

        _unitOfWork.ArgumentRepository.Add(arguments);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return arguments;
    }
    /// <summary>
    ///   Update information with argument
    /// </summary>
    /// <param name="request">information need update argument</param>
    /// <param name="cancellationToken">token to cancellation action</param>
    /// <returns>
    ///     Return argument response after updated success
    /// </returns>

    public async Task<Arguments> UpdateArgumentAsync(UpdateArgumentResponse request,
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
        await TestArgumentWithQueryAsync(arguments);

        _unitOfWork.ArgumentRepository.Update(arguments);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return arguments;
    }
    /// <summary>
    ///     Try to test query with get argument
    /// </summary>
    /// <param name="arguments">argument need test query</param>
    private async Task TestArgumentWithQueryAsync(Arguments arguments)
    {
        string[] paramInQuery = arguments.Query.GetAllArgumentsInterpolateInTemplate();
        var parameters = new ExpandoObject() as IDictionary<string, object?>;
        foreach (var p in paramInQuery)
        {
            parameters[p] = null;
        }
        try
        {
            _ = await _connectionQueryServices.QueryFirstOrDefaultAsync<object>(arguments.Query, parameters);
        }
        catch
        {
            ThrowHelper.ThrowWhenBusinessError(ArgumentConstMessage.InvalidQuery);
        }
    }

}