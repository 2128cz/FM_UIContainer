using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FullMotionUIContainer.Runtime.Container
{  
    /// <summary>
    /// 容器感知接口，由容器实现，容器内子项调用；
    /// 从容器可以选择到子项，通过容器感知接口，使得子项可以将自身被选择的型号传递给容器。
    /// </summary>
    [Obsolete("实际看来可以不用")]
    public interface IContainerAwareness 
    {
        
    }
}