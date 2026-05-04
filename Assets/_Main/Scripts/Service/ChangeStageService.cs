using System.Collections;
using UnityEngine.InputSystem;

[AutoService(typeof(ISceneBeginBootService), typeof(ISceneBeginUnloadService))]
public class ChangeStageService : ISceneBeginBootService, ISceneBeginUnloadService
{
    private Input _input;
    private string _stageLoaderEntityId;
    
    public IEnumerator OnSceneBeginBoot()
    {
        _input = G.Resolve<Input>();
        
        _input.Debug.StageLoaderScreen.Enable();
        _input.Debug.StageLoaderScreen.performed += OnStageLoaderScreenPerformed;
        yield return null;
    }

    private void OnStageLoaderScreenPerformed(InputAction.CallbackContext ctx)
    {
        
    }

    public IEnumerator OnSceneBeginUnload()
    {
        _input.Debug.StageLoaderScreen.performed -= OnStageLoaderScreenPerformed;
        _input.Debug.StageLoaderScreen.Disable();
        yield return null;
    }
}