using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class FoldoutPanel : MonoBehaviour
    {
        [Header("折叠面板设置")]
        public RectTransform contentArea;
        public Button toggleButton;
        public Text headerText;

        [Header("动画设置")]
        public float animationDuration = 0.3f;
        public AnimationCurve expandCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public AnimationCurve collapseCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("图标设置")]
        public Text toggleIcon;
        public string expandIcon = "▼";
        public string collapseIcon = "▶";

        private bool isExpanded = true;
        private float targetHeight;
        private Coroutine animateCoroutine;

        void Awake()
        {
            if (contentArea == null || toggleButton == null)
            {
                Debug.LogError("FoldoutPanel: 请确保已分配contentArea和toggleButton引用");
                return;
            }

            // 初始化高度
            targetHeight = contentArea.sizeDelta.y;

            // 设置初始状态
            SetInitialState();

            // 绑定事件
            toggleButton.onClick.AddListener(Toggle);
        }

        void SetInitialState()
        {
            if (headerText != null && string.IsNullOrEmpty(headerText.text))
            {
                headerText.text = "折叠面板";
            }

            if (toggleIcon != null)
            {
                toggleIcon.text = expandIcon;
            }
        }

        public void Toggle()
        {
            isExpanded = !isExpanded;
            UpdateToggleUI();

            // 停止之前的动画协程
            if (animateCoroutine != null)
            {
                StopCoroutine(animateCoroutine);
            }

            // 启动新动画
            animateCoroutine = StartCoroutine(AnimateContent());
        }

        void UpdateToggleUI()
        {
            if (toggleIcon != null)
            {
                toggleIcon.text = isExpanded ? expandIcon : collapseIcon;
            }

            if (headerText != null)
            {
                Color textColor = isExpanded ? Color.white : new Color(0.7f, 0.7f, 0.7f);
                headerText.color = textColor;
            }
        }

        IEnumerator AnimateContent()
        {
            float startHeight = contentArea.sizeDelta.y;
            float endHeight = isExpanded ? targetHeight : 0;
            float duration = animationDuration;
            float elapsed = 0;

            AnimationCurve curve = isExpanded ? expandCurve : collapseCurve;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float progress = Mathf.Clamp01(elapsed / duration);
                float curvedProgress = curve.Evaluate(progress);

                float currentHeight = Mathf.Lerp(startHeight, endHeight, curvedProgress);
                contentArea.sizeDelta = new Vector2(contentArea.sizeDelta.x, currentHeight);

                yield return null;
            }

            // 确保最终值准确
            contentArea.sizeDelta = new Vector2(contentArea.sizeDelta.x, endHeight);
            contentArea.gameObject.SetActive(isExpanded);

            animateCoroutine = null;
        }

        public void SetExpanded(bool expanded)
        {
            if (isExpanded == expanded) return;

            isExpanded = expanded;
            UpdateToggleUI();

            if (animateCoroutine != null)
            {
                StopCoroutine(animateCoroutine);
            }

            //float height = isExpanded ? targetHeight : 0;
            //contentArea.sizeDelta = new Vector2(contentArea.sizeDelta.x, height);

            contentArea.gameObject.SetActive(expanded);
        }
    }
}