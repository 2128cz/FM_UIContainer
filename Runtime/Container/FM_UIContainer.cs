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
	/// 具有动态驱动的可选择的UI容器
	/// <code>
	/// 这里只实现容器功能，不实现容器内具体的操作；
	/// 可能更多会使用动态生成的方式实现容器成员，其中的互动项目可以通过索引来完成交互。
	/// 因此更推荐让其中的按钮部分的逻辑只完成展示的效果；
	/// 具体的执行逻辑可以放在调用按钮互动的时候，这样可能会离数据更近更好交互。
	/// </code>
	/// </summary>
	/// <typeparam name="T">要求容器内的成员必须要有一个管理器，对于已经存在的对象，将自动添加</typeparam>
	public abstract class FM_UIContainer : FM_UIBase
	{
		#region 字段属性

		/// <summary>
		/// 容器内项目计数
		/// </summary>
		public int Count => childUIList?.Count ?? 0;

		/// <summary>
		/// ui对象池
		/// </summary>
		private ObjectPool<GameObject> UIPool;

		/// <summary>
		/// ui容器中的子成员
		/// </summary>
		private List<FM_UISelectable> childUIList;

		/// <summary>
		/// 选中UI对象，一个容器中应当只有一个选中的成员
		/// </summary>
		private FM_UISelectable selectTarget;


		/// <summary>
		/// 获取当前选中的UI对象
		/// </summary>
		public FM_UISelectable SelectTarget => selectTarget;

		#endregion

		#region 容器方法

		/// <summary>
		/// 绑定特定节点下所有UI
		/// </summary>
		/// <typeparam name="T">可互动组件类型</typeparam>
		/// <param name="targetTransform">关注组件所处父节点</param>
		/// <param name="SubRecursion">寻找深度</param>
		/// <param name="compPostprocess">找到后执行后处理</param>
		protected void BindChildrenUI<T>(Transform targetTransform, int SubRecursion = 2, Action<T> compPostprocess = null)
		where T : FM_UISelectable
		{
			bool candoProcess = compPostprocess != null;

			if(childUIList == null)
				childUIList = new List<FM_UISelectable>();

			foreach(var comp in targetTransform.GetChildrenRecursionComponents<T>(SubRecursion))
			{
				if(comp == null)
					Debug.Log($"{transform.parent.name} -> {transform.name} 查找{targetTransform.name}中的项目引用为空");
				else
				{
					if(candoProcess)
						compPostprocess(comp);
					BindUIBase(comp);
				}
			}
		}

		/// <summary>
		/// 绑定子项下所有UI
		/// </summary>
		/// <typeparam name="T">可互动组件类型</typeparam>
		/// <param name="SubRecursion">寻找深度</param>
		/// <param name="compPostprocess">找到后执行后处理</param>
		protected void BindChildrenUI<T>(int SubRecursion = 2, Action<T> compPostprocess = null)
		where T : FM_UISelectable
		{
			BindChildrenUI<T>(transform, SubRecursion, compPostprocess);
		}

		/// <summary>
		/// 绑定一个ui项目作为自己的子项，此处会进行基本的有效性检查，以及容器检查
		/// </summary>
		/// <param name="ui"></param>
		protected void BindUIBase(FM_UISelectable ui)
		{
			if(childUIList == null || ui == null || childUIList.Contains(ui))
			{
				Debug.LogError($"初始化时{(childUIList==null?"列表为空":"列表有效")}，{(ui == null?"UI为空":"UI有效")}，{(childUIList.Contains(ui) ? "列表已经包含子项":"新项目")}");
				return;
			}

			// 设定此ui的父组件到此
			ui.ParentContainer = this;
			childUIList.Add(ui);

			ui.WhenHasUISelected += Ui_WhenHasUISelected;
			ui.WhenHasUIDiselected += Ui_WhenHasUIDiselected;
		}

		/// <summary>
		/// 取消所有成员的绑定
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
		/// 取消一个成员的绑定
		/// </summary>
		/// <param name="ui"></param>
		protected void UnbindUIBase(FM_UISelectable ui)
		{
			childUIList.Remove(ui);
			ui.WhenHasUISelected -= Ui_WhenHasUISelected;
			ui.WhenHasUIDiselected -= Ui_WhenHasUIDiselected;
		}


		/// <summary>
		/// 当ui项目并非由容器选中时，由项目发送的回调事件;
		/// 项目中应当进行一次重复性验证，只有当初次选中时才应该执行选中
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
		/// 当ui项目不再选中时，由项目发送的回调事件
		/// </summary>
		/// <param name="obj"></param>
		/// <exception cref="NotImplementedException"></exception>
		private void Ui_WhenHasUIDiselected(FM_UISelectable obj)
		{
			if(selectTarget == obj)
			selectTarget = null;
		}


		/// <summary>
		/// 选择一个UI条目
		/// </summary>
		/// <param name="targetIndex"></param>
		public virtual void SelectedUI(int targetIndex)
		{
			if(selectTarget != null)
				selectTarget.OnUIDeselected(this);

			if(childUIList == null)
			{
				childUIList = new List<FM_UISelectable>();
				throw new ($"在进行选择时，项目并未初始化，请确保进行了初始化比如：BindChildrenUI");
			}
			if(targetIndex < 0 || targetIndex >= childUIList.Count)
			{
				Debug.LogError($"在进行选择时，项目超出可选择范围：{targetIndex}");
				return;
			}
#if _DEBUG_
			Debug.Log($"选择项目：{targetIndex} / {childUIList.Count}");
#endif

			// 标记选中
			selectTarget = childUIList[targetIndex];

			if(selectTarget != null)
				selectTarget.OnUISelected(this);
		}
		
		/// <summary>
		/// 选择一个UI条目
		/// </summary>
		/// <param name="target"></param>
		public virtual void SelectedUI(FM_UISelectable target)
		{
			if(selectTarget != null)
				selectTarget.OnUIDeselected(this);

			// 标记选中
			selectTarget = target;

			if(selectTarget != null)
				selectTarget.OnUISelected(this);
		}

		/// <summary>
		/// 退出选中项目
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
		/// 与选中的项目进行互动
		/// </summary>
		public virtual FM_UISelectable PressedSelectedUI()
		{
			if(selectTarget != null)
				selectTarget.OnPressed();
			return selectTarget;
		}

		/// <summary>
		/// 与选中的项目进行互动
		/// </summary>
		/// <param name="index">返回这个按钮在列表中的索引，用于脱离按钮执行</param>
		/// <returns></returns>
		public virtual FM_UISelectable PressedSelectedUI(out int index)
		{
			index = childUIList.IndexOf(selectTarget);
			return PressedSelectedUI();
		}

		/// <summary>
		/// 与选中的项目取消互动
		/// </summary>
		public virtual FM_UISelectable ReleasedSelectedUI()
		{
			if(selectTarget != null)
				selectTarget.OnReleased();
			return selectTarget;
		}



		/// <summary>
		/// 完成滚动效果
		/// </summary>
		/// <param name="scroll">滚动方向及速率</param>
		/// <param name="rate">滚动速率倍数、乘数</param>
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
		/// 设置滚动
		/// </summary>
		/// <param name="scroll">滚动方向及速率</param>
		/// <param name="rate">滚动速率倍数、乘数</param>
		/// <param name="adsorb">吸附强度，0为不吸附</param>
		public void SetScrollGearDamping(float scroll, float rate, float gearDamping)
		{
			
		}

		#endregion

	}
}