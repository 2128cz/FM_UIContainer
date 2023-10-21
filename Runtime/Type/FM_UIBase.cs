#define _DEUBG_

#if UNITY_EDITOR
#undef _DEBUG_
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif

using UnityEngine;
using UnityEngine.UI;



namespace FullMotionUIContainer.Runtime.Type
{
    /// <summary>
    /// ��̬UI����
    /// </summary>
    public class FM_UIBase : MonoBehaviour
	{
		#region �ֶ�����

		/// <summary>
		/// �ɼ��ľ��α任��Χ�����Խ��˶������ڵ�Ԫ����
		/// </summary>
		[SerializeField]
		private RectTransform _visibleRect;

		/// <summary>
		/// �˴���UI�任���
		/// </summary>
		private RectTransform _rectTransform;

		/// <summary>
		/// ��ʼ����
		/// </summary>
		private Vector3 init_position;

		/// <summary>
		/// ��ʼ��ת
		/// </summary>
		private Quaternion init_rotate;

		/// <summary>
		/// ��ʼ����
		/// </summary>
		private Vector3 init_localScale;



		/// <summary>
		/// �ɼ��Ա任��������Զ���Ϊ�ڵ�����ı任��Χ��
		/// ����Ϊ�գ�����Ҫ���пռ��
		/// </summary>
		public RectTransform visibleRect
		{
			get
			{
				if(_visibleRect == null)
					_visibleRect = transform.GetComponentInParent<Mask>().rectTransform;
				return _visibleRect;
			}
		}

		/// <summary>
		/// �˴���UI�任���
		/// </summary>
		public RectTransform rectTransform
		{
			get
			{
				if(_rectTransform == null)
					_rectTransform = transform.GetComponent<RectTransform>();
				return _rectTransform;
			}
		}

		/// <summary>
		/// ��ʼ����
		/// </summary>
		public Vector3 Init_position => init_position;

		/// <summary>
		/// ��ʼ��ת
		/// </summary>
		public Quaternion Init_rotate => init_rotate;

		/// <summary>
		/// ��ʼ����
		/// </summary>
		public Vector3 Init_localScale => init_localScale;



		/// <summary>
		/// ������
		/// </summary>
		private FM_UIBase parentContainer;

		/// <summary>
		/// ������
		/// </summary>
		public FM_UIBase ParentContainer
		{
			get => parentContainer;
			internal set => parentContainer = value;
		}



#if UNITY_EDITOR
		/// <summary>
		/// �˴��Ľű�������Ϣ
		/// </summary>
		internal List<(string, MessageType)> ScriptMes = null;
#endif

		#endregion

		#region ��������

		protected virtual void Awake()
		{
			SetInitTransform();
		}

		#endregion

		#region ����

#if UNITY_EDITOR
		/// <summary>
		/// ���һ���˴��Ľű���Ϣ����ͬʱ����Ϣ�������������Ϣ
		/// </summary>
		/// <param name="context"></param>
		/// <param name="type"></param>
		internal void DebugMes(string context, MessageType type = 0)
		{
			if(ScriptMes == null)
				ScriptMes = new List<(string, MessageType)>();
			switch(type)
			{
				case MessageType.None:
				case MessageType.Info:
				Debug.Log(context);
				break;
				case MessageType.Warning:
				Debug.LogWarning(context);
				break;
				case MessageType.Error:
				Debug.LogError(context);
				break;
			}
			ScriptMes.Add((context, type));
		}
#endif

		/// <summary>
		/// �жϸ����ľ��α任����Ƿ���ȫ���˴�������
		/// </summary>
		/// <param name="target"></param>
		/// <returns>������������ȣ�����Ϊ�棬��Ϊ�ײ��а������ж��Ǵ��ڵ��ڡ�</returns>
		protected bool CheckContained(RectTransform target)
		{
			if(visibleRect == target)
				return true;
			return Library.IsRectTransformFullyContained(visibleRect, target);
		}



		/// <summary>
		/// ���ó�ʼ��ת
		/// </summary>
		protected void SetInitTransform()
		{
			init_position = transform.position;
			init_rotate = transform.rotation;
			init_localScale = transform.localScale;
		}

		#endregion

		#region ת��

		public static implicit operator bool(FM_UIBase lhs)
        {
            return lhs == null;
        }

		public static implicit operator RectTransform(FM_UIBase lhs)
		{
			return lhs.rectTransform;
		}

		#endregion
	}


#if UNITY_EDITOR

	/// <summary>
	/// ������λ��
	/// </summary>
	[CustomEditor(typeof(FM_UIBase))] 
	public class RectTransformCheckEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			FM_UIBase targetComponent = (FM_UIBase)target;

			if(targetComponent.GetComponent<RectTransform>() == null)
			{
				EditorGUILayout.HelpBox("����ӵ�� RectTransform ����� GameObject ����Ӵ������", MessageType.Error);

				//if(GUILayout.Button("�Զ���� RectTransform ���"))
				//{
				//	targetComponent.gameObject.AddComponent<RectTransform>();
				//}
			}
			else
			{
				base.OnInspectorGUI();

				if(targetComponent.ScriptMes != null)
				{
					foreach(var mes in targetComponent.ScriptMes)
					{
						EditorGUILayout.HelpBox(mes.Item1, mes.Item2);
					}
				}
			}
		}
	}
#endif

}