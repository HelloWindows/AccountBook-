﻿using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.RectTransform {
    public class JTweenRectTransformAnchorMin : JTweenBase {
        private Vector2 m_beginAnchorMin = Vector2.zero;
        private Vector2 m_toAnchorMin = Vector2.zero;
        private UnityEngine.RectTransform m_RectTransform;

        public JTweenRectTransformAnchorMin() {
            m_tweenType = (int)JTweenRectTransform.AnchorMin;
            m_tweenElement = JTweenElement.RectTransform;
        }

        public Vector2 BeginAnchorMin {
            get {
                return m_beginAnchorMin;
            }
            set {
                m_beginAnchorMin = value;
            }
        }

        public Vector2 ToAnchorMin {
            get {
                return m_toAnchorMin;
            }
            set {
                m_toAnchorMin = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_RectTransform = m_target.GetComponent<UnityEngine.RectTransform>();
            if (null == m_RectTransform) return;
            // end if
            m_beginAnchorMin = m_RectTransform.anchorMax;
        }

        protected override Tween DOPlay() {
            if (null == m_RectTransform) return null;
            // end if
            return m_RectTransform.DOAnchorMax(m_toAnchorMin, m_duration, m_isSnapping);
        }

        public override void Restore() {
            if (null == m_RectTransform) return;
            // end if
            m_RectTransform.anchorMax = m_beginAnchorMin;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginAnchorMin")) BeginAnchorMin = JTweenUtils.JsonToVector2(json.GetNode("beginAnchorMin"));
            // end if
            if (json.Contains("anchorMin")) m_toAnchorMin = JTweenUtils.JsonToVector2(json.GetNode("anchorMin"));
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("beginAnchorMin", JTweenUtils.Vector2Json(m_beginAnchorMin));
            json.SetNode("anchorMin", JTweenUtils.Vector2Json(m_toAnchorMin));
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_RectTransform) {
                errorInfo = GetType().FullName + " GetComponent<RectTransform> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
