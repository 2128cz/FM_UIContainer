#define _DEBUG_

#if UNITY_EDITOR
#undef _DEBUG_
#endif

using FullMotionUIContainer.Runtime.Type;

using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

namespace FullMotionUIContainer.Runtime.Container
{
	/// <summary>
	/// ���ж�̬�����Ŀ�ѡ���UI����
	/// <code>
	/// ����ֻʵ���������ܣ���ʵ�������ھ���Ĳ�����
	/// ���ܸ����ʹ�ö�̬���ɵķ�ʽʵ��������Ա�����еĻ�����Ŀ����ͨ����������ɽ�����
	/// ��˸��Ƽ������еİ�ť���ֵ��߼�ֻ���չʾ��Ч����
	/// �����ִ���߼����Է��ڵ��ð�ť������ʱ���������ܻ������ݸ������ý�����
	/// </code>
	/// </summary>
	/// <typeparam name="T">Ҫ�������ڵĳ�Ա����Ҫ��һ���������������Ѿ����ڵĶ��󣬽��Զ����</typeparam>
	public abstract class FM_UIContainer : FM_UIBase
	{
		#region �ֶ�����

		/// <summary>
		/// ��������Ŀ����
		/// </summary>
		public int Count => childUIList?.Count ?? 0;

		/// <summary>
		/// ui�����
		/// </summary>
		private ObjectPool<GameObject> UIPool;

		/// <summary>
		/// ui�����е��ӳ�Ա
		/// </summary>
		private List<FM_UISelectable> childUIList;

		/// <summary>
		/// ѡ��UI����һ��������Ӧ��ֻ��һ��ѡ�еĳ�Ա
		/// </summary>
		private FM_UISelectable selectTarget;


		/// <summary>
		/// ��ȡ��ǰѡ�е�UI����
		/// </summary>
		public FM_UISelectable SelectTarget => selectTarget;

		#endregion

		#region ��������

		/// <summary>
		/// ���ض��ڵ�������UI
		/// </summary>
		/// <typeparam name="T">�ɻ����������</typeparam>
		/// <param name="targetTransform">��ע����������ڵ�</param>
		/// <param name="SubRecursion">Ѱ�����</param>
		/// <param name="compPostprocess">�ҵ���ִ�к���</param>
		protected void BindChildrenUI<T>(Transform targetTransform, int SubRecursion = 2, Action<T> compPostprocess = null)
		where T : FM_UISelectable
		{
			bool candoProcess = compPostprocess != null;

			if(childUIList == null)
				childUIList = new List<FM_UISelectable>();

			foreach(var comp in targetTransform.GetChildrenRecursionComponents<T>(SubRecursion))
			{
				if(comp == null)
					Debug.Log($"{transform.parent.name} -> {transform.name} ����{targetTransform.name}�е���Ŀ����Ϊ��");
				else
				{
					if(candoProcess)
						compPostprocess(comp);
					BindUIBase(comp);
				}
			}
		}

		/// <summary>
		/// ������������UI
		/// </summary>
		/// <typeparam name="T">�ɻ����������</typeparam>
		/// <param name="SubRecursion">Ѱ�����</param>
		/// <param name="compPostprocess">�ҵ���ִ�к���</param>
		protected void BindChildrenUI<T>(int SubRecursion = 2, Action<T> compPostprocess = null)
		where T : FM_UISelectable
		{
			BindChildrenUI<T>(transform, SubRecursion, compPostprocess);
		}

		/// <summary>
		/// ��һ��ui��Ŀ��Ϊ�Լ�������˴�����л�������Ч�Լ�飬�Լ��������
		/// </summary>
		/// <param name="ui"></param>
		protected void BindUIBase(FM_UISelectable ui)
		{
			if(childUIList == null || ui == null || childUIList.Contains(ui))
			{
				Debug.LogError($"��ʼ��ʱ{(childUIList==null?"�б�Ϊ��":"�б���Ч")}��{(ui == null?"UIΪ��":"UI��Ч")}��{(childUIList.Contains(ui) ? "�б��Ѿ���������":"����Ŀ")}");
				return;
			}

			// �趨��ui�ĸ��������
			ui.ParentContainer = this;
			childUIList.Add(ui);

			ui.WhenHasUISelected += Ui_WhenHasUISelected;
			ui.WhenHasUIDiselected += Ui_WhenHasUIDiselected;
		}

		/// <summary>
		/// ȡ�����г�Ա�İ�
		/// </summary>
		protected void UnbindAllUIBase()
		{
			foreach(var ui in childUIList
			.Where(a => a != null))
			{
				ui.WhenHasUISelected -= Ui_WhenHasUISelected;
				ui.WhenHasUIDiselected -= Ui_WhenHasUIDiselected;
			}
			childUIList.Clear();
		}

		/// <summary>
		/// ȡ��һ����Ա�İ�
		/// </summary>
		/// <param name="ui"></param>
		protected void UnbindUIBase(FM_UISelectable ui)
		{
			childUIList.Remove(ui);
			ui.WhenHasUISelected -= Ui_WhenHasUISelected;
			ui.WhenHasUIDiselected -= Ui_WhenHasUIDiselected;
		}


		/// <summary>
		/// ��ui��Ŀ����������ѡ��ʱ������Ŀ���͵Ļص��¼�;
		/// ��Ŀ��Ӧ������һ���ظ�����֤��ֻ�е�����ѡ��ʱ��Ӧ��ִ��ѡ��
		/// </summary>
		/// <param name="obj"></param>
		/// <exception cref="NotImplementedException"></exception>
		private void Ui_WhenHasUISelected(FM_UISelectable obj)
		{
			if(selectTarget != obj && selectTarget != null)
				selectTarget.OnUIDeselected(this);
			selectTarget = obj;
		}

		/// <summary>
		/// ��ui��Ŀ����ѡ��ʱ������Ŀ���͵Ļص��¼�
		/// </summary>
		/// <param name="obj"></param>
		/// <exception cref="NotImplementedException"></exception>
		private void Ui_WhenHasUIDiselected(FM_UISelectable obj)
		{
			if(selectTarget == obj)
			selectTarget = null;
		}


		/// <summary>
		/// ѡ��һ��UI��Ŀ
		/// </summary>
		/// <param name="targetIndex"></param>
		public virtual void SelectedUI(int targetIndex)
		{
			if(selectTarget != null)
				selectTarget.OnUIDeselected(this);

			if(childUIList == null)
			{
				childUIList = new List<FM_UISelectable>();
				throw new ($"�ڽ���ѡ��ʱ����Ŀ��δ��ʼ������ȷ�������˳�ʼ�����磺BindChildrenUI");
			}
			if(targetIndex < 0 || targetIndex >= childUIList.Count)
			{
				Debug.LogError($"�ڽ���ѡ��ʱ����Ŀ������ѡ��Χ��{targetIndex}");
				return;
			}
#if _DEBUG_
			Debug.Log($"ѡ����Ŀ��{targetIndex} / {childUIList.Count}");
#endif

			// ���ѡ��
			selectTarget = childUIList[targetIndex];

			if(selectTarget != null)
				selectTarget.OnUISelected(this);
		}
		
		/// <summary>
		/// ѡ��һ��UI��Ŀ
		/// </summary>
		/// <param name="target"></param>
		public virtual void SelectedUI(FM_UISelectable target)
		{
			if(selectTarget != null)
				selectTarget.OnUIDeselected(this);

			// ���ѡ��
			selectTarget = target;

			if(selectTarget != null)
				selectTarget.OnUISelected(this);
		}

		/// <summary>
		/// �˳�ѡ����Ŀ
		/// </summary>
		public virtual FM_UIBase DeselectedUI()
		{
			if(selectTarget != null)
				selectTarget.OnUIDeselected(this);

			var result = selectTarget;
			selectTarget = null;
			return result;
		}

		/// <summary>
		/// ��ѡ�е���Ŀ���л���
		/// </summary>
		public virtual FM_UISelectable PressedSelectedUI()
		{
			if(selectTarget != null)
				selectTarget.OnPressed();
			return selectTarget;
		}

		/// <summary>
		/// ��ѡ�е���Ŀ���л���
		/// </summary>
		/// <param name="index">���������ť���б��е��������������밴ťִ��</param>
		/// <returns></returns>
		public virtual FM_UISelectable PressedSelectedUI(out int index)
		{
			index = childUIList.IndexOf(selectTarget);
			return PressedSelectedUI();
		}

		/// <summary>
		/// ��ѡ�е���Ŀȡ������
		/// </summary>
		public virtual FM_UISelectable ReleasedSelectedUI()
		{
			if(selectTarget != null)
				selectTarget.OnReleased();
			return selectTarget;
		}



		/// <summary>
		/// ��ɹ���Ч��
		/// </summary>
		/// <param name="scroll">������������</param>
		/// <param name="rate">�������ʱ���������</param>
		protected void UpdateScroll(float scroll, float rate)
		{
			for(int i = 0; i < childUIList.Count; i++)
			{
				var uitem = childUIList[i];

				if(CheckContained(uitem))
				{
					
				}
			}
			
		}
		
		/// <summary>
		/// ���ù���
		/// </summary>
		/// <param name="scroll">������������</param>
		/// <param name="rate">�������ʱ���������</param>
		/// <param name="adsorb">����ǿ�ȣ�0Ϊ������</param>
		public void SetScrollGearDamping(float scroll, float rate, float gearDamping)
		{
			
		}

		#endregion

	}
}