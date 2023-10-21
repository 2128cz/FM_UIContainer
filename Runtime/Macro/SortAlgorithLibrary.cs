using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FullMotionUIContainer.Runtime.Macro
{
    /// <summary>
    /// �����㷨��
    /// </summary>
    public static class SortAlgorithLibrary
    {
		/// <summary>
		/// ����������У����ж��������������У������������Ϊ����
		/// </summary>
		/// <param name="index">˳������</param>
		/// <param name="axis">��������</param>
		/// <param name="separatingDis">����Ĵ�ֱ����</param>
		/// <param name="interval">��Ŀ֮��ľ���</param>
		/// <returns>����һ���ֲ��ռ������</returns>
		public static Vector2 AlongAxisSide2(int index, Vector2 axis, float separatingDis, float interval)
        {
			var axis_normal = axis.normalized;
			var axis_sideOffset = Vector2.Perpendicular(axis_normal) * separatingDis;
			var item_pos = index * axis_normal * interval;
			return axis_sideOffset + item_pos;
		}

		/// <summary>
		/// ����������У����ж��������������У������������Ϊ����
		/// </summary>
		/// <param name="index">˳������</param>
		/// <param name="axis">��������</param>
		/// <param name="offset">ԭʼƫ����</param>
		/// <param name="separatingDis">����Ĵ�ֱ����</param>
		/// <param name="interval">��Ŀ֮��ľ���</param>
		/// <returns>����һ���ֲ��ռ������</returns>
		public static Vector2 AlongAxisSide2(int index, Vector2 axis, Vector2 offset, float separatingDis, float interval)
        {
			return offset + AlongAxisSide2(index, axis, separatingDis, interval);
		}

		/// <summary>
		/// ����������У����ж�����ԭ������
		/// </summary>
		/// <param name="index">˳������</param>
		/// <param name="amount">��Ŀ����</param>
		/// <param name="axis">��������</param>
		/// <param name="separatingDis">����Ĵ�ֱ����</param>
		/// <param name="interval">��Ŀ֮��ľ���</param>
		/// <returns>����һ���ֲ��ռ������</returns>
		public static Vector2 AlongAxisSide2(int index, int amount, Vector2 axis, float separatingDis, float interval)
        {
			if(amount <= 0)
			{
				Debug.LogError("�ڼ���UI����ʱ����������������0");
				return Vector2.zero;
			}
			var axis_normal = axis.normalized;
			var axis_sideOffset = Vector2.Perpendicular(axis_normal) * separatingDis;
			var item_pos = index * axis_normal * interval;
			var origin = amount * axis_normal * interval / 2;
			return axis_sideOffset + item_pos - origin;
		}

		/// <summary>
		/// ����������У����ж�����ԭ������
		/// </summary>
		/// <param name="index">˳������</param>
		/// <param name="amount">��Ŀ����</param>
		/// <param name="axis">��������</param>
		/// <param name="separatingDis">����Ĵ�ֱ����</param>
		/// <param name="interval">��Ŀ֮��ľ���</param>
		/// <returns>����һ���ֲ��ռ������</returns>
		public static Vector2 AlongAxisSide2(int index, int amount, Vector2 axis, Vector2 offset, float separatingDis, float interval)
        {
			return offset + AlongAxisSide2(index, amount, axis, separatingDis, interval);
		}

	}
}