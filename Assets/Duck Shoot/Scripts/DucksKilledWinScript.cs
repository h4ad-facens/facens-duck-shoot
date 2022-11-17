using TMPro;
using UnityEngine;

namespace Duck_Shoot
{
    public class DucksKilledWinScript : MonoBehaviour
    {
        public TextMeshProUGUI WinText;

        void Start()
        {
            WinText.text = $"Você matou {StaticGameVariables.DucksKilled} patos";
        }
    }
}