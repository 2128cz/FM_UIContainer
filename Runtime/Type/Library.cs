using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace FullMotionUIContainer.Runtime.Type
{
    /// <summary>
    /// UI库
    /// </summary>
    public static class Library 
    {
        /// <summary>
        /// 获取一个变换节点下所有的子成员
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static IEnumerable<Transform> GetChildren(this Transform trans)
        {
            for(int i = 0; i < trans.childCount; i++)
            {
                yield return trans.GetChild(i);
            }
        }

		/// <summary>
		/// 在所有子成员中深度优先找到第一个组件
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="trans"></param>
		/// <param name="recursion">遍历深度，0表示仅在此处寻找，1表示在包含下一级在内的所有成员中寻找</param>
		/// <returns></returns>
		public static IEnumerable<T> GetChildrenRecursionComponents<T>(this Transform trans, int recursion = 1)
		where T : Component
		{
			int newRecursion = recursion - 1;

			foreach(var item in trans.GetChildren())
			{
				var comp = item.GetComponent<T>();
				if(comp == null && newRecursion > 0)
					comp = item.GetChildrenRecursionComponent<T>(newRecursion);
				yield return comp;
			}
		}

		/// <summary>
		/// 按深度优先寻找一个组件
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="trans"></param>
		/// <param name="depth">遍历深度，0表示仅在此处寻找，1表示在包含下一级在内的所有成员中寻找</param>
		/// <returns></returns>
		public static T GetChildrenRecursionComponent<T>(this Transform trans, int depth = 1)
		where T : Component
		{
			T result;
			int newDepth = depth - 1;

#if true	// 如果不知道这里在找什么的可以打开调试看看
			foreach(var item in trans.GetChildren())
			{
				result = item.GetComponent<T>();
				if(result == null && newDepth >= 0)
					result = item.GetChildrenRecursionComponent<T>(newDepth);
				if(result != null)
					return result;
			}
#else
			string info = $"{depth}.   开始在{trans.name}中寻找\r\n";
			foreach(var item in trans.GetChildren())
			{
				info += $"{depth}. > 正在{item.name}中寻找\r\n";

				result = item.GetComponent<T>();

				info += $"{depth}.   当前节点{(result == null ? "不包含组件":"包含组件")}\r\n";

				if(result == null && newDepth >= 0)
				{
					info += $"{depth}.   前往深层寻找 > \r\n";
					result = item.GetRecursionComponent<T>(newDepth);
					info += $"{depth}.   深层查找返回 < \r\n";
				}

				if(result != null)
				{
					info += $"{depth}.   已找到结果：{result.name} \r\n";
					Debug.Log(info);
					return result;
				}
				info += $"{depth}.   没有找到任何东西 \r\n";
			}
			Debug.Log(info + "没有找到，空结束");
#endif
			return null;
		}

		/// <summary>
		/// 检查一个矩形变换组件是否完全包含另一个举行变换组件
		/// </summary>
		/// <param name="outerRect">检查包含的组件</param>
		/// <param name="innerRect">被包含的组件</param>
		/// <returns></returns>
		public static bool IsRectTransformFullyContained(RectTransform outerRect, RectTransform innerRect)
		{
			Rect outerBounds = GetRectTransformBounds(outerRect);
			Rect innerBounds = GetRectTransformBounds(innerRect);

			return outerBounds.Contains(innerBounds.min) && outerBounds.Contains(innerBounds.max);
		}

		/// <summary>
		/// 获取一个矩形变换组件的矩形包围
		/// </summary>
		/// <param name="rectTransform"></param>
		/// <returns></returns>
		public static Rect GetRectTransformBounds(RectTransform rectTransform)
		{
			Vector3[] corners = new Vector3[4];
			rectTransform.GetWorldCorners(corners);

			float minX = Mathf.Min(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
			float minY = Mathf.Min(corners[0].y, corners[1].y, corners[2].y, corners[3].y);
			float maxX = Mathf.Max(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
			float maxY = Mathf.Max(corners[0].y, corners[1].y, corners[2].y, corners[3].y);

			return new Rect(minX, minY, maxX - minX, maxY - minY);
		}

	}
}