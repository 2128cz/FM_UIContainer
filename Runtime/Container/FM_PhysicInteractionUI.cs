using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FullMotionUIContainer.Runtime.Container
{
	/// <summary>
	/// �������UI�ؼ���һ��ȡ�ɵķ�����ֱ������ʹ�� Physics.Raycast ���UI��⣬�����ڽ�Ϊ�򵥵�UI����
	/// </summary>
	public abstract class FM_PhysicInteractionUI : FM_UISelectable
	{
		#region �ֶ�����

		/// <summary>
		/// UI��ײ�壬һ����˵�Ƿ��ε�
		/// </summary>
		private Collider ui_Collider;

		#endregion

		#region ��������

		protected override void Awake()
		{
			base.Awake();
			ui_Collider = GetComponent<Collider>();
			if(ui_Collider == null)
			{
				var collider = gameObject.AddComponent<BoxCollider>();
				ui_Collider = collider;
				UpdateColliderSizeToRect();
			}
		}

		#endregion

		#region ����

		/// <summary>
		/// ���´˴���UI��ײ��ߴ絽���α任���
		/// </summary>
		protected virtual void UpdateColliderSizeToRect()
		{
			if (ui_Collider is BoxCollider collider)
				collider.size = new Vector3(this.rectTransform.sizeDelta.x, this.rectTransform.sizeDelta.y, 1);
		}

		#endregion

		#region ������

		/// <summary>
		/// ������
		/// </summary>
		public virtual void CurrentOnPlace()
		{
			OnUISelected();
			// ������ʹ�ã���Ϊû�гɶԵ�ȡ���¼�
			//if(SelectableTarget is Button button)
			//	button.OnSelect(new BaseEventData(EventSystem.current));
		}

		/// <summary>
		/// ��껥��/���밴ť
		/// </summary>
		public virtual void CurrentOnClick()
		{
			if(SelectableTarget is Button button)
			{
				button.onClick?.Invoke();
			}
		}

		#endregion

		#region ��ײ���

		/**
		 * ��ť������Ӹ���ȸ��ӵļ��������
		 * �������������ײ�¼���
		 * ���Կ��Ժ�����ײ��⣻
		 * �������ܿ���is trigger
		 */

		//private void OnCollisionEnter(Collision collision)
		//{ }

		//private void OnCollisionExit(Collision collision)
		//{ }

		//private void OnTriggerEnter(Collider other)
		//{ }

		//private void OnTriggerExit(Collider other)
		//{ }

		#endregion

	}



	/// <summary>
	/// ��չ������UI
	/// </summary>
	public static partial class FM_PhysicInteractionUI_Extent
	{
		/// <summary>
		/// ��ȡ������������UI
		/// </summary>
		/// <param name="collider"></param>
		public static FM_PhysicInteractionUI GetPhysicUI(this Collider collider)
		{
			var target = collider.GetComponent<FM_PhysicInteractionUI>();
			if(target == null)
				Debug.LogError("�޷���ȡ���е�������UI���");
			return target;
		}
	}
}