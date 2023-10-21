#define _DEUBG_
#define _WARING_

#if UNITY_EDITOR
#undef _DEBUG_
#endif

using FullMotionUIContainer.Runtime.Type;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace FullMotionUIContainer.Runtime.Container
{
	/// <summary>
	/// UIԪ���࣬���ڰ�ť�Ȼ���Ԫ��
	/// </summary>
    public abstract class FM_UISelectable : FM_UIBase
	{

		/// <summary>
		/// ��Ԫ�صĹ�����������������
		/// </summary>
		//protected internal FM_UIContainer ManagingContainer;

		#region �¼�ί��

		/// <summary>
		/// ����һ��UI��Ŀ��ѡ��ʱ
		/// </summary>
		public event Action<FM_UISelectable> WhenHasUISelected;

		/// <summary>
		/// ����һ��UI��Ŀ��ȡ��ѡ��ʱ
		/// </summary>
		public event Action<FM_UISelectable> WhenHasUIDiselected;

		#endregion

		#region �ֶ�����

		/// <summary>
		/// �¼�ϵͳ
		/// </summary>
		public static EventSystem UIEventSystem = null;

		/// <summary>
		/// �¼�ϵͳ�Ҳ���ʱ�����ٲ��ң�������ڴ���UIʱ������һ�������ڵ�������в���
		/// </summary>
		private static bool UIEventSystemNotFind = false;

		/// <summary>
		/// Ĭ��ͼ��
		/// </summary>
		private Sprite normalImage;
		/// <summary>
		/// ��ͨͼ��
		/// </summary>
		public Sprite NormalImage => normalImage;
		/// <summary>
		/// ����ͼ��
		/// </summary>
		public Sprite HighlightedImage => SelectableTarget.spriteState.highlightedSprite;
		/// <summary>
		/// ����ͼ��
		/// </summary>
		public Sprite PressedImage => SelectableTarget.spriteState.pressedSprite;
		/// <summary>
		/// ѡ��ͼ��
		/// </summary>
		public Sprite SelectedSprite => SelectableTarget.spriteState.selectedSprite;
		/// <summary>
		/// ����ͼ��
		/// </summary>
		public Sprite DisabledImage => SelectableTarget.spriteState.disabledSprite;

		/// <summary>
		/// ��ťͼ��
		/// </summary>
		private UnityEngine.UI.Image image;

		/// <summary>
		/// ��ťͼ��
		/// </summary>
		public UnityEngine.UI.Image Image => image;


		/// <summary>
		/// �ɽ����Ķ���
		/// </summary>
		protected internal Selectable SelectableTarget;

		#endregion

		#region ��������

		protected override void Awake()
		{
			base.Awake();

			// ��ʼ���ɽ�������
			SelectableTarget = transform.GetComponent<Selectable>();
			if(SelectableTarget == null)
			{
				Debug.LogError($"FMUI���棺δ����ͬ�㼶���ҵ�һ����ѡ���UI����FMUI������Ҫ�����������");
			}
			else
			{
				// ��ť����ͼ��
				image = SelectableTarget.GetComponent<UnityEngine.UI.Image>();
				if(image != null)
					image = SelectableTarget.GetComponentInChildren<UnityEngine.UI.Image>();
				if(image != null)
					normalImage = image.sprite;
			}
			
			// �����¼�ϵͳ
			if(!UIEventSystemNotFind && UIEventSystem == null)
			{
				UIEventSystem = FindObjectOfType<EventSystem>();
				if(UIEventSystem == null)
					UIEventSystemNotFind = true;
			}
		}

		#endregion

		#region ����

		/**
		 * ����ԭ����ť�¼�
		 */
#if false
		public void OnPointerDown()
		{
			PointerEventData eventData = new PointerEventData(EventSystem.current);
			eventData.pointerEnter = SelectableTarget.gameObject;
			SelectableTarget.OnPointerDown(eventData);
		}
		public void OnPointerDown(PointerEventData eventData)
		{
			SelectableTarget.OnPointerDown(eventData);
		}

		public void OnPointerUp()
		{
			PointerEventData eventData = new PointerEventData(EventSystem.current);
			eventData.pointerEnter = SelectableTarget.gameObject;
			SelectableTarget.OnPointerUp(eventData);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			SelectableTarget.OnPointerUp(eventData);
		}

		public void OnPointerEnter()
		{
			PointerEventData eventData = new PointerEventData(EventSystem.current);
			eventData.pointerEnter = SelectableTarget.gameObject;
			SelectableTarget.OnPointerEnter(eventData);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			SelectableTarget.OnPointerEnter(eventData);
		}

		public void OnPointerExit()
		{
			PointerEventData eventData = new PointerEventData(EventSystem.current);
			eventData.pointerEnter = SelectableTarget.gameObject;
			SelectableTarget.OnPointerExit(eventData);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			SelectableTarget.OnPointerExit(eventData);
		}

		public void OnSelect()
		{
			PointerEventData eventData = new PointerEventData(EventSystem.current);
			eventData.pointerEnter = SelectableTarget.gameObject;
			SelectableTarget.OnSelect(eventData);
		}

		public void OnSelect(PointerEventData eventData)
		{
			SelectableTarget.OnSelect(eventData);
		}

		public void OnDeselect()
		{
			PointerEventData eventData = new PointerEventData(EventSystem.current);
			eventData.pointerEnter = SelectableTarget.gameObject;
			SelectableTarget.OnDeselect(eventData);
		}

		public void OnDeselect(PointerEventData eventData)
		{
			SelectableTarget.OnDeselect(eventData);
		}

		public void Select()
		{
			SelectableTarget.Select();
		}

#endif

		/**
		 * ������ť�¼�
		 */

		/// <summary>
		/// ����Ŀ��ѡ��ʱ����ʾ����Ŀ���Ը���
		/// </summary>
		/// <param name="container">��������������ã�������ṩ�������ã��ұ�֤����Ŀ������������ʼ��</param>
		protected internal virtual void OnUISelected(FM_UIBase container = null)
		{
#if _DEBUG_
			Debug.Log($"ѡ��{this.name}");
#endif
			// ��������������͵�ѡ���¼�����ô����һ��ѡ���¼�
			if(container != null && ParentContainer != null && container == ParentContainer)
				return;
			WhenHasUISelected?.Invoke(this);
		}

		/// <summary>
		/// ����Ŀ���ٱ�ѡ��ʱ����ʾ����Ŀ���ٸ���
		/// </summary>
		/// <param name="container">��������������ã�������ṩ�������ã��ұ�֤����Ŀ������������ʼ��</param>
		protected internal virtual void OnUIDeselected(FM_UIBase container = null)
		{
#if _DEBUG_
			Debug.Log($"ȡ��ѡ��{this.name}");
#endif
			// ��������������͵�ȡ��ѡ���¼�����ô����һ��ȡ��ѡ���¼�
			if(container != null && ParentContainer != null && container == ParentContainer)
				return;
			WhenHasUIDiselected?.Invoke(this);
		}


		protected internal abstract void OnPressed();

		protected internal abstract void OnReleased();


		#endregion
	}

	public static partial class FM_PhysicInteractionUI_Extent
	{
		/// <summary>
		/// ��ȡ���пɽ�����UI
		/// </summary>
		/// <param name="collider"></param>
		public static Selectable GetSelectableUI(this Collider collider)
		{
			return collider.GetComponent<Selectable>();
		}

		/// <summary>
		/// ��ȡ���пɽ����İ�ť
		/// </summary>
		/// <param name="collider"></param>
		public static UnityEngine.UI.Button GetButtonUI(this Collider collider)
		{
			return collider.GetComponent<UnityEngine.UI.Button>();
		}
	}
}