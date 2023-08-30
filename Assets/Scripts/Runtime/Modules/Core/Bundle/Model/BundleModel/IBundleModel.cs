using Runtime.Modules.Core.PromiseTool;

namespace Runtime.Modules.Core.Bundle.Model.BundleModel
{
  public interface IBundleModel
  {
    IPromise<T> LoadAssetAsync<T>(string key);
  }
}