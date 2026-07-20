using System.Collections;
using UnityEngine;

namespace CivSandbox.Tooling
{
    [AddComponentMenu("")]
    [DefaultExecutionOrder(-32000)]
    public sealed class BootstrapPlayerExit : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return null;
            Debug.Log("CIV001-SMOKE-000: Bootstrap player reached its first frame.");
            Application.Quit(0);
        }
    }
}
