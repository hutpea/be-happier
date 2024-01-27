using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameVolumeManager : MonoBehaviour
{
    public Volume volume;

    private VolumeParameter<float> saturation = new VolumeParameter<float>();
    private VolumeParameter<float> constrast = new VolumeParameter<float>();
    
    public ColorAdjustments CA;
    
    private void Awake()
    { 
        volume.profile.TryGet<ColorAdjustments>(out CA);
        if (CA == null)
        {
            Debug.LogError("No ColorAdjustments found on profile");
        }
    }

    [Button]
    public void ResetToNormalMode()
    {
        saturation.value = 20F;
        constrast.value = 5F;
        CA.saturation.SetValue(saturation);
        CA.contrast.SetValue(constrast);
    }
    
    [Button]
    public void ResetToBlackWhiteMode()
    {
        DOTween.To(() => saturation.value, x => saturation.value = x, -100f, 1.5f).OnUpdate(() =>
        {
            CA.saturation.SetValue(saturation);
        });
        DOTween.To(() => constrast.value, x => constrast.value = x, -5f, 0.5f).OnUpdate(() =>
        {
            CA.saturation.SetValue(constrast);
        });
    }
}
