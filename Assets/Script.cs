public class CScript : UnityEngine.MonoBehaviour{
    private UnityEngine.Transform VPlayer;
    private UnityEngine.Camera VCamera;
    private UnityEngine.Vector3 VRotator;
    private UnityEngine.Vector3 VRotation;
    private UnityEngine.Vector3 VTranslator;
    private UnityEngine.Vector3 VTranslation;

    public enum EMode{
        VFirstPersonShooter ,
        VIsometry
    }
    
    public EMode VMode;
    public float VSensitivity;
    public float VSpeed;

    public void Start(){
        UnityEngine.Cursor.visible = false;
        VPlayer = GetComponentsInParent<UnityEngine.Transform>()[1];
        VCamera = GetComponent<UnityEngine.Camera>();
        switch(VMode){
            case EMode.VFirstPersonShooter:
                VCamera.orthographic = false;
                VCamera.fieldOfView = UnityEngine.Camera.HorizontalToVerticalFieldOfView(120.0F , VCamera.aspect);
                VCamera.usePhysicalProperties = false;
                VRotation = VPlayer.eulerAngles;
                transform.localEulerAngles = VRotation;
                VTranslation = VPlayer.position;
                transform.localPosition = new UnityEngine.Vector3(0.0F , 0.0F, 0.0F);
            break;
            case EMode.VIsometry:
                VCamera.orthographic = true;
                VCamera.orthographicSize = 100.0F;
                VRotation = new UnityEngine.Vector3(45.0F , 45.0F , 0.0F);
                transform.localEulerAngles = VRotation;
                VTranslation = VPlayer.position;
                transform.localPosition = new UnityEngine.Vector3(-7.5F , 10.0F, -7.5F);
            break;
        }
    }

    public void Update(){
        switch(VMode){
            case EMode.VFirstPersonShooter:
                VRotation += VRotator * VSensitivity;
                if(VRotation.y < 0.0F){
                    VRotation.y += 360.0F;
                }
                if(VRotation.y >= 360.0F){
                    VRotation.y -= 360.0F;
                }
                VRotation.x = UnityEngine.Mathf.Clamp(VRotation.x , -90.0F , 90.0F);
                transform.localEulerAngles = VRotation;
                VTranslation.x += UnityEngine.Mathf.Sin((VRotation.y + 90.0F) * UnityEngine.Mathf.Deg2Rad) * VTranslator.x * VSpeed * UnityEngine.Time.deltaTime;
                VTranslation.z += UnityEngine.Mathf.Cos((VRotation.y + 90.0F) * UnityEngine.Mathf.Deg2Rad) * VTranslator.x * VSpeed * UnityEngine.Time.deltaTime;
                VTranslation.x += UnityEngine.Mathf.Sin(VRotation.y * UnityEngine.Mathf.Deg2Rad) * VTranslator.z * VSpeed * UnityEngine.Time.deltaTime;
                VTranslation.z += UnityEngine.Mathf.Cos(VRotation.y * UnityEngine.Mathf.Deg2Rad) * VTranslator.z * VSpeed * UnityEngine.Time.deltaTime;
                VPlayer.position = VTranslation;
            break;
            case EMode.VIsometry:
                VTranslation.x += VTranslator.x * VSpeed * UnityEngine.Time.deltaTime;
                VTranslation.z -= VTranslator.x * VSpeed * UnityEngine.Time.deltaTime;
                VTranslation.x += VTranslator.z * VSpeed * UnityEngine.Time.deltaTime;
                VTranslation.z += VTranslator.z * VSpeed * UnityEngine.Time.deltaTime;
                VPlayer.position = VTranslation;
            break;
        }
    }

    public void FRotate(UnityEngine.InputSystem.InputAction.CallbackContext PContext)
    {
        if(PContext.performed){
            UnityEngine.Vector2 LRotator = PContext.ReadValue<UnityEngine.Vector2>();
            VRotator = new UnityEngine.Vector3(-LRotator.y , LRotator.x , 0.0F);
            return;
        }
        if(PContext.canceled){
            VRotator = new UnityEngine.Vector3(0.0F , 0.0F , 0.0F);
            return;
        }
    }

    public void FTranslate(UnityEngine.InputSystem.InputAction.CallbackContext PContext)
    {
        if(PContext.performed){
            UnityEngine.Vector2 LTranslator = PContext.ReadValue<UnityEngine.Vector2>();
            VTranslator = new UnityEngine.Vector3(LTranslator.x , 0.0F , LTranslator.y);
            return;
        }
        if(PContext.canceled){
            VTranslator = new UnityEngine.Vector3(0.0F , 0.0F , 0.0F);
            return;
        }
    }
    
    public void FToggleCameraMode(UnityEngine.InputSystem.InputAction.CallbackContext PContext){
        if(PContext.performed){
            VMode = 1 - VMode;
            Start();
        }
    }
}
