using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace FullMotionUIContainer.Runtime.Type
{
    /// <summary>
    /// UI��
    /// </summary>
    public static class Library 
    {
        /// <summary>
        /// ��ȡһ���任�ڵ������е��ӳ�Ա
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
		/// �������ӳ�Ա����������ҵ���һ�����
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="trans"></param>
		/// <param name="recursion">������ȣ�0��ʾ���ڴ˴�Ѱ�ң�1��ʾ�ڰ�����һ�����ڵ����г�Ա��Ѱ��</param>
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
		/// ���������Ѱ��һ�����
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="trans"></param>
		/// <param name="depth">������ȣ�0��ʾ���ڴ˴�Ѱ�ң�1��ʾ�ڰ�����һ�����ڵ����г�Ա��Ѱ��</param>
		/// <returns></returns>
		public static T GetChildrenRecursionComponent<T>(this Transform trans, int depth = 1)
		where T : Component
		{
			T result;
			int newDepth = depth - 1;

#if true	// �����֪����������ʲô�Ŀ��Դ򿪵��Կ���
			foreach(var item in trans.GetChildren())
			{
				result = item.GetComponent<T>();
				if(result == null && newDepth >= 0)
					result = item.GetChildrenRecursionComponent<T>(newDepth);
				if(result != null)
					return result;
			}
#else
			string info = $"{depth}.   ��ʼ��{trans.name}��Ѱ��\r\n";
			foreach(var item in trans.GetChildren())
			{
				info += $"{depth}. > ����{item.name}��Ѱ��\r\n";

				result = item.GetComponent<T>();

				info += $"{depth}.   ��ǰ�ڵ�{(result == null ? "���������":"�������")}\r\n";

				if(result == null && newDepth >= 0)
				{
					info += $"{depth}.   ǰ�����Ѱ�� > \r\n";
					result = item.GetRecursionComponent<T>(newDepth);
					info += $"{depth}.   �����ҷ��� < \r\n";
				}

				if(result != null)
				{
					info += $"{depth}.   ���ҵ������{result.name} \r\n";
					Debug.Log(info);
					return result;
				}
				info += $"{depth}.   û���ҵ��κζ��� \r\n";
			}
			Debug.Log(info + "û���ҵ����ս���");
#endif
			return null;
		}

		/// <summary>
		/// ���һ�����α任����Ƿ���ȫ������һ�����б任���
		/// </summary>
		/// <param name="outerRect">�����������</param>
		/// <param name="innerRect">�����������</param>
		/// <returns></returns>
		public static bool IsRectTransformFullyContained(RectTransform outerRect, RectTransform innerRect)
		{
			Rect outerBounds = GetRectTransformBounds(outerRect);
			Rect innerBounds = GetRectTransformBounds(innerRect);

			return outerBounds.Contains(innerBounds.min) && outerBounds.Contains(innerBounds.max);
		}

		/// <summary>
		/// ��ȡһ�����α任����ľ��ΰ�Χ
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