public class Script : UnityEngine.MonoBehaviour{
    private UnityEngine.Vector3 VTranslator;
    private UnityEngine.Vector3 VTranslation;
    private UnityEngine.Vector3 VRotator;
    private UnityEngine.Vector3 VRotation;
    
    public float VSpeed;
    public float VSensitivity;

    public void Start(){
        VTranslation = transform.position;
        VRotation = transform.eulerAngles;
    }

    public void Update(){
        VTranslation += VTranslator * VSpeed * UnityEngine.Time.deltaTime;
        transform.position = VTranslation;
        VRotation += VRotator * VSensitivity;
        if(VRotation.y < 0.0F){
            VRotation.y += 360.0F;
        }
        if(VRotation.y >= 360.0F){
            VRotation.y -= 360.0F;
        }
        VRotation.x = UnityEngine.Mathf.Clamp(VRotation.x , -90.0F , 90.0F);
        transform.eulerAngles = VRotation;
    }

    public void FTranslate(UnityEngine.InputSystem.InputAction.CallbackContext PContext)
    {
        if(PContext.started){
            UnityEngine.Vector2 LTranslator = PContext.ReadValue<UnityEngine.Vector2>();
            VTranslator = new UnityEngine.Vector3(LTranslator.x , 0.0F , LTranslator.y);
            VTranslator = transform.localToWorldMatrix.MultiplyVector(VTranslator);
            VTranslator.y = 0.0F;
            VTranslator.Normalize();
            return;
        }
        if(PContext.canceled){
            VTranslator = new UnityEngine.Vector3(0.0F , 0.0F , 0.0F);
            return;
        }
    }

    public void FRotate(UnityEngine.InputSystem.InputAction.CallbackContext PContext)
    {
        if(PContext.started){
            UnityEngine.Vector2 LRotator = PContext.ReadValue<UnityEngine.Vector2>();
            VRotator = new UnityEngine.Vector3(-LRotator.y , LRotator.x , 0.0F);
            return;
        }
        if(PContext.canceled){
            VRotator = new UnityEngine.Vector3(0.0F , 0.0F , 0.0F);
            return;
        }
 }
}
