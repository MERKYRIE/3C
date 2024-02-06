namespace N3C{
    internal class CScript : UnityEngine.MonoBehaviour{
        private enum EMode{
            VFirstPersonShooter ,
            VIsometry
        }

        private UnityEngine.Transform VPlayer;
        private UnityEngine.Camera VCamera;
        private UnityEngine.Vector3 VRotator;
        private UnityEngine.Vector3 VRotation;
        private UnityEngine.Vector3 VTranslator;
        private UnityEngine.Vector3 VTranslation;

        [UnityEngine.SerializeField]private EMode VMode;
        [UnityEngine.SerializeField]private float VFieldOfView;
        [UnityEngine.SerializeField]private bool VUsePhysicalProperties;
        [UnityEngine.SerializeField]private float VSize;
        [UnityEngine.SerializeField]private UnityEngine.Vector3 VRotationByDefault;
        [UnityEngine.SerializeField]private UnityEngine.Vector3 VTranslationByDefault;
        [UnityEngine.SerializeField]private UnityEngine.Vector2 VSensitivity;
        [UnityEngine.SerializeField]private float VSpeed;

        private void Start(){
            if(UnityEngine.Cursor.visible){
                UnityEngine.Cursor.visible = false;
            }
            if(!VPlayer){
                VPlayer = GetComponentsInParent<UnityEngine.Transform>()[1];
            }
            if(!VCamera){
                VCamera = GetComponent<UnityEngine.Camera>();
            }
            switch(VMode){
                case EMode.VFirstPersonShooter:
                    VCamera.orthographic = false;
                    VCamera.fieldOfView = UnityEngine.Camera.HorizontalToVerticalFieldOfView(VFieldOfView , VCamera.aspect);
                    VCamera.usePhysicalProperties = VUsePhysicalProperties;
                    VRotation = VPlayer.eulerAngles;
                    transform.localEulerAngles = VRotation;
                    VTranslation = VPlayer.position;
                    transform.localPosition = new UnityEngine.Vector3(0.0F , 0.0F, 0.0F);
                break;
                case EMode.VIsometry:
                    VCamera.orthographic = true;
                    VCamera.orthographicSize = VSize;
                    VRotation = VRotationByDefault;
                    transform.localEulerAngles = VRotation;
                    VTranslation = VPlayer.position;
                    transform.localPosition = VTranslationByDefault;
                break;
            }
        }

        private void Update(){
            switch(VMode){
                case EMode.VFirstPersonShooter:
                    VRotation.x += VRotator.x * VSensitivity.x;
                    VRotation.y += VRotator.y * VSensitivity.y;
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

        private void FRotate(UnityEngine.InputSystem.InputAction.CallbackContext PContext)
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

        private void FTranslate(UnityEngine.InputSystem.InputAction.CallbackContext PContext)
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
    
        private void FToggleCameraMode(UnityEngine.InputSystem.InputAction.CallbackContext PContext){
            if(PContext.performed){
                VMode = 1 - VMode;
                Start();
            }
        }
    }
}