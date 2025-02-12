﻿using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.Transform {
    public class JTweenTransformLocalJump : JTweenBase {
        private Vector3 m_beginPosition = Vector3.zero;
        private Vector3 m_toPosition = Vector3.zero;
        private int m_numJumps = 0;
        private float m_jumpPower = 0;
        private UnityEngine.Transform m_Transform;

        public JTweenTransformLocalJump() {
            m_tweenType = (int)JTweenTransform.LocalJump;
            m_tweenElement = JTweenElement.Transform;
        }

        public Vector3 BeginPosition {
            get {
                return m_beginPosition;
            }
            set {
                m_beginPosition = value;
            }
        }

        public Vector3 ToPosition {
            get {
                return m_toPosition;
            }
            set {
                m_toPosition = value;
            }
        }

        public float JumpPower {
            get {
                return m_jumpPower;
            }
            set {
                m_jumpPower = value;
            }
        }

        public int NumJumps {
            get {
                return m_numJumps;
            }
            set {
                m_numJumps = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_Transform = m_target.GetComponent<UnityEngine.Transform>();
            if (null == m_Transform) return;
            // end if
            m_beginPosition = m_Transform.localPosition;
        }

        protected override Tween DOPlay() {
            if (null == m_Transform) return null;
            // end if
            return m_Transform.DOLocalJump(m_toPosition, m_jumpPower, m_numJumps, m_duration, m_isSnapping);
        }

        public override void Restore() {
            if (null == m_Transform) return;
            // end if
            m_Transform.localPosition = m_beginPosition;
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginPosition")) BeginPosition = JTweenUtils.JsonToVector3(json.GetNode("beginPosition"));
            // end if
            if (json.Contains("endValue")) m_toPosition = JTweenUtils.JsonToVector3(json.GetNode("endValue"));
            // end if
            if (json.Contains("jumpPower")) m_jumpPower = json.GetFloat("jumpPower");
            // end if
            if (json.Contains("numJumps")) m_numJumps = json.GetInt("numJumps");
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("beginPosition", JTweenUtils.Vector3Json(m_beginPosition));
            json.SetNode("endValue", JTweenUtils.Vector3Json(m_toPosition));
            json.SetFloat("jumpPower", m_jumpPower);
            json.SetInt("numJumps", m_numJumps);
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_Transform) {
                errorInfo = GetType().FullName + " GetComponent<Transform> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
