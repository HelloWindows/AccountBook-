﻿using Json;
using DG.Tweening;
using UnityEngine;

namespace JTween.Camera {
    public class JTweenCameraPixelRect : JTweenBase {
        private Rect m_beginPixelRect = Rect.zero;
        private Rect m_toPixelRect = Rect.zero;
        private UnityEngine.Camera m_Camera;

        public JTweenCameraPixelRect() {
            m_tweenType = (int)JTweenCamera.PixelRect;
            m_tweenElement = JTweenElement.Camera;
        }

        public Rect BeginPixelRect {
            get {
                return m_beginPixelRect;
            }
            set {
                m_beginPixelRect = value;
            }
        }

        public Rect ToPixelRect {
            get {
                return m_toPixelRect;
            }
            set {
                m_toPixelRect = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Camera = m_target.GetComponent<UnityEngine.Camera>();
            if (null == m_Camera) return;
            // end if
            m_beginPixelRect = m_Camera.pixelRect;
        }

        protected override Tween DOPlay() {
            if (null == m_Camera) return null;
            // end if
            return m_Camera.DOPixelRect(m_toPixelRect, m_duration);
        }

        public override void Restore() {
            if (null == m_Camera) return;
            // end if
            m_Camera.pixelRect = m_beginPixelRect;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginPixelRect")) {
                Vector4 rect = JTweenUtils.JsonToVector4(json.GetNode("beginPixelRect"));
                 m_beginPixelRect = new Rect(rect.x, rect.y, rect.z, rect.w);
            } // end if
            if (json.Contains("pixelRect")) {
                Vector4 rect = JTweenUtils.JsonToVector4(json.GetNode("pixelRect"));
                m_toPixelRect = new Rect(rect.x, rect.y, rect.z, rect.w);
            } // end if
        }

        protected override void ToJson(ref IJsonNode json) {
            Vector4 rect = new Vector4(m_beginPixelRect.x, m_beginPixelRect.y, m_beginPixelRect.width, m_beginPixelRect.height);
            json.SetNode("beginPixelRect", JTweenUtils.Vector4Json(rect)); 
            rect = new Vector4(m_toPixelRect.x, m_toPixelRect.y, m_toPixelRect.width, m_toPixelRect.height);
            json.SetNode("pixelRect", JTweenUtils.Vector4Json(rect));
            Restore();
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_Camera) {
                errorInfo = GetType().FullName + " GetComponent<Camera> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
