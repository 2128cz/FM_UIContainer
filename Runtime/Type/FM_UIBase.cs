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
    /// 动态UI基础
    /// </summary>
    public class FM_UIBase : MonoBehaviour
	{
		#region 字段属性

		/// <summary>
		/// 可见的矩形变换范围，可以将此定义至遮挡元素上
		/// </summary>
		[SerializeField]
		private RectTransform _visibleRect;

		/// <summary>
		/// 此处的UI变换组件
		/// </summary>
		private RectTransform _rectTransform;

		/// <summary>
		/// 初始坐标
		/// </summary>
		private Vector3 init_position;

		/// <summary>
		/// 初始旋转
		/// </summary>
		private Quaternion init_rotate;

		/// <summary>
		/// 初始缩放
		/// </summary>
		private Vector3 init_localScale;



		/// <summary>
		/// 可见性变换组件，可以定义为遮挡组件的变换范围。
		/// 可能为空，必须要进行空检查
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
		/// 此处的UI变换组件
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
		/// 初始坐标
		/// </summary>
		public Vector3 Init_position => init_position;

		/// <summary>
		/// 初始旋转
		/// </summary>
		public Quaternion Init_rotate => init_rotate;

		/// <summary>
		/// 初始缩放
		/// </summary>
		public Vector3 Init_localScale => init_localScale;



		/// <summary>
		/// 父容器
		/// </summary>
		private FM_UIBase parentContainer;

		/// <summary>
		/// 父容器
		/// </summary>
		public FM_UIBase ParentContainer
		{
			get => parentContainer;
			internal set => parentContainer = value;
		}



#if UNITY_EDITOR
		/// <summary>
		/// 此处的脚本错误信息
		/// </summary>
		internal List<(string, MessageType)> ScriptMes = null;
#endif

		#endregion

		#region 生命周期

		protected virtual void Awake()
		{
			SetInitTransform();
		}

		#endregion

		#region 方法

#if UNITY_EDITOR
		/// <summary>
		/// 添加一个此处的脚本信息，并同时在信息面板中输出这个信息
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
		/// 判断给定的矩形变换组件是否完全被此处包含。
		/// </summary>
		/// <param name="target"></param>
		/// <returns>如果两者组件相等，返回为真，因为底层中包含的判断是大于等于。</returns>
		protected bool CheckContained(RectTransform target)
		{
			if(visibleRect == target)
				return true;
			return Library.IsRectTransformFullyContained(visibleRect, target);
		}



		/// <summary>
		/// 设置初始旋转
		/// </summary>
		protected void SetInitTransform()
		{
			init_position = transform.position;
			init_rotate = transform.rotation;
			init_localScale = transform.localScale;
		}

		#endregion

		#region 转换

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
	/// 检查放置位置
	/// </summary>
	[CustomEditor(typeof(FM_UIBase))] 
	public class RectTransformCheckEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			FM_UIBase targetComponent = (FM_UIBase)target;

			if(targetComponent.GetComponent<RectTransform>() == null)
			{
				EditorGUILayout.HelpBox("请在拥有 RectTransform 组件的 GameObject 上添加此组件！", MessageType.Error);

				//if(GUILayout.Button("自动添加 RectTransform 组件"))
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