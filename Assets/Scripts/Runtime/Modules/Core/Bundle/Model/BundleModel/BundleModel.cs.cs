using Runtime.Modules.Core.PromiseTool;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Modules.Core.Bundle.Model.BundleModel
{
  public class BundleModel : IBundleModel
  {
    public IPromise<T> LoadAssetAsync<T>(string key)
    {
      Promise<T> promise = new();
      AsyncOperationHandle<T> op = Addressables.LoadAssetAsync<T>(key);
      
      op.Completed += handle =>
      {
        if (handle.Status == AsyncOperationStatus.Succeeded)
          promise.Resolve(handle.Result);
        else
          promise.Reject(handle.OperationException);
      };
      
      return promise;
    }
    
    // public IPromise InitAndLoadAll()
    // {
    //   Promise promise = new();
    //   AsyncOperationHandle<IResourceLocator> asyncOperationHandle = Addressables.InitializeAsync();
    //   
    //   asyncOperationHandle.Completed += handle =>
    //   {
    //     if (handle.Status == AsyncOperationStatus.Succeeded)
    //     {
    //       contextView.GetComponent<MonoBehaviour>().StartCoroutine(LoadBundles(promise));
    //     }
    //
    //     else
    //       promise.Reject(handle.OperationException);
    //   };
    //   return promise;
    // }
  }
}