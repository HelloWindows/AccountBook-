﻿using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.Rigidbody {
    public class JTweenRigidbodyRotate : JTweenBase {
        private Vector3 m_beginRotate = Vector3.zero;
        private Vector3 m_toRotate = Vector3.zero;
        private RotateMode m_RotateMode = RotateMode.Fast;
        private UnityEngine.Rigidbody m_Rigidbody;

        public JTweenRigidbodyRotate() {
            m_tweenType = (int)JTweenRigidbody.Rotate;
            m_tweenElement = JTweenElement.Rigidbody;
        }

        public Vector3 BeginRotate {
            get {
                return m_beginRotate;
            }
            set {
                m_beginRotate = value;
            }
        }

        public Vector3 ToRotate {
            get {
                return m_toRotate;
            }
            set {
                m_toRotate = value;
            }
        }

        public RotateMode RotateMode {
            get {
                return m_RotateMode;
            }
            set {
                m_RotateMode = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Rigidbody = m_target.GetComponent<UnityEngine.Rigidbody>();
            if (null == m_Rigidbody) return;
            // end if
            m_beginRotate = m_Rigidbody.rotation.eulerAngles;
        }

        protected override Tween DOPlay() {
            if (null == m_Rigidbody) return null;
            // end if
            return m_Rigidbody.DORotate(m_toRotate, m_duration, m_RotateMode);
        }

        public override void Restore() {
            if (null == m_Rigidbody) return;
            // end if
            m_Rigidbody.rotation = Quaternion.Euler(m_beginRotate);
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginRotate")) BeginRotate = JTweenUtils.JsonToVector3(json.GetNode("beginRotate"));
            // end if
            if (json.Contains("rotate")) m_toRotate = JTweenUtils.JsonToVector3(json.GetNode("rotate"));
            // end if
            if (json.Contains("mode")) m_RotateMode = (RotateMode)(int)json.GetInt("mode");
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("beginRotate", JTweenUtils.Vector3Json(m_beginRotate));
            json.SetNode("rotate", JTweenUtils.Vector3Json(m_toRotate));
            json.SetInt("mode", (int)m_RotateMode);
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_Rigidbody) {
                errorInfo = GetType().FullName + " GetComponent<Rigidbody> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
