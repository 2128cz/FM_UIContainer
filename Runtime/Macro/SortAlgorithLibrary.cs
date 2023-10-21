using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FullMotionUIContainer.Runtime.Macro
{
    /// <summary>
    /// 排序算法库
    /// </summary>
    public static class SortAlgorithLibrary
    {
		/// <summary>
		/// 沿着轴测排列，所有东西沿正方向排列（如果给定距离为正）
		/// </summary>
		/// <param name="index">顺序索引</param>
		/// <param name="axis">沿轴排列</param>
		/// <param name="separatingDis">与轴的垂直距离</param>
		/// <param name="interval">项目之间的距离</param>
		/// <returns>返回一个局部空间的坐标</returns>
		public static Vector2 AlongAxisSide2(int index, Vector2 axis, float separatingDis, float interval)
        {
			var axis_normal = axis.normalized;
			var axis_sideOffset = Vector2.Perpendicular(axis_normal) * separatingDis;
			var item_pos = index * axis_normal * interval;
			return axis_sideOffset + item_pos;
		}

		/// <summary>
		/// 沿着轴测排列，所有东西沿正方向排列（如果给定距离为正）
		/// </summary>
		/// <param name="index">顺序索引</param>
		/// <param name="axis">沿轴排列</param>
		/// <param name="offset">原始偏移量</param>
		/// <param name="separatingDis">与轴的垂直距离</param>
		/// <param name="interval">项目之间的距离</param>
		/// <returns>返回一个局部空间的坐标</returns>
		public static Vector2 AlongAxisSide2(int index, Vector2 axis, Vector2 offset, float separatingDis, float interval)
        {
			return offset + AlongAxisSide2(index, axis, separatingDis, interval);
		}

		/// <summary>
		/// 沿着轴测排列，所有东西沿原点排列
		/// </summary>
		/// <param name="index">顺序索引</param>
		/// <param name="amount">项目总数</param>
		/// <param name="axis">沿轴排列</param>
		/// <param name="separatingDis">与轴的垂直距离</param>
		/// <param name="interval">项目之间的距离</param>
		/// <returns>返回一个局部空间的坐标</returns>
		public static Vector2 AlongAxisSide2(int index, int amount, Vector2 axis, float separatingDis, float interval)
        {
			if(amount <= 0)
			{
				Debug.LogError("在计算UI排序时，排列总数不大于0");
				return Vector2.zero;
			}
			var axis_normal = axis.normalized;
			var axis_sideOffset = Vector2.Perpendicular(axis_normal) * separatingDis;
			var item_pos = index * axis_normal * interval;
			var origin = amount * axis_normal * interval / 2;
			return axis_sideOffset + item_pos - origin;
		}

		/// <summary>
		/// 沿着轴测排列，所有东西沿原点排列
		/// </summary>
		/// <param name="index">顺序索引</param>
		/// <param name="amount">项目总数</param>
		/// <param name="axis">沿轴排列</param>
		/// <param name="separatingDis">与轴的垂直距离</param>
		/// <param name="interval">项目之间的距离</param>
		/// <returns>返回一个局部空间的坐标</returns>
		public static Vector2 AlongAxisSide2(int index, int amount, Vector2 axis, Vector2 offset, float separatingDis, float interval)
        {
			return offset + AlongAxisSide2(index, amount, axis, separatingDis, interval);
		}

	}
}