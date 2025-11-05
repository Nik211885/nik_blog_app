using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Extend;
/// <summary>
///  Extend services support upload file to cloudinary
/// </summary>
public class CloudinaryServices
{
    private readonly CloudinaryConfigDataModel _cloudinaryConfigDataModel;
    private readonly Cloudinary _cloudinary;
    public CloudinaryServices(IConfiguration configuration)
    {
        CloudinaryConfigDataModel? cloudinaryConfigDataModel
            = configuration.GetSection("Cloudinary").Get<CloudinaryConfigDataModel>();
        ArgumentNullException.ThrowIfNull(cloudinaryConfigDataModel);
        _cloudinaryConfigDataModel = cloudinaryConfigDataModel;
        Account account = new()
        {
            Cloud = _cloudinaryConfigDataModel.CloudName,
            ApiKey = _cloudinaryConfigDataModel.ApiKey,
            ApiSecret = _cloudinaryConfigDataModel.ApiSecret,
        };
        _cloudinary = new Cloudinary(account);
    }
    /// <summary>
    ///     Get url with signature includes api key and api secret for upload file
    ///     to cloudinary and default expired for url is 5 minute and just backend and
    ///     cloudinary server should verify signature
    /// </summary>
    /// <returns>
    ///     Return with url for upload file to cloudinary includes signature 
    /// </returns>
    public string GetUrlUploadFileBySignature()
    {
        var timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var parameters = new SortedDictionary<string, object>
        {
            {"folder", _cloudinaryConfigDataModel.UploadFolder },
            {"timestamp", timeStamp},
        };
        var stringSign = string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        var signature = _cloudinary.Api.SignParameters(parameters);
        var urlWithSignature =
            string.Concat(_cloudinaryConfigDataModel.UrlUpload,
                string.Format("/{0}/image/upload?api_key={1}&{2}&signature={3}",
                    _cloudinaryConfigDataModel.CloudName,
                    _cloudinaryConfigDataModel.ApiKey,
                    stringSign,
                    signature));
        return urlWithSignature;
    }
}

/// <summary>
///  Data model about config cloudinary
/// </summary>
internal class CloudinaryConfigDataModel
{
    /// <summary>
    ///     Url cloudinary for upload file 
    /// </summary>
    public string UrlUpload { get; set; } = string.Empty;
    /// <summary>
    ///     Name your cloudinary to upload
    /// </summary>
    public string CloudName { get; set; } = string.Empty;
    /// <summary>   
    ///     Api key for your cloudinary
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;
    /// <summary>
    ///     Secret in configuration in cloudinary
    /// </summary>
    public string ApiSecret { get; set; } = string.Empty;
    /// <summary>
    ///     Folder you have config in cloudinary
    /// </summary>
    public string UploadFolder { get; set; } = string.Empty;
}