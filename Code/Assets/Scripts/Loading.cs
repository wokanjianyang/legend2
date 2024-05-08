using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Loading : MonoBehaviour
    {

        public Text Txt_Desc;

        void Awake()
        {

        }

        public void SetText(string text)
        {
            Txt_Desc.text = text;
        }
    }
}
