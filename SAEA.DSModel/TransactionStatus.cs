/****************************************************************************
*项目名称：SAEA.DSModel
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：SAEA.DSModel
*类 名 称：TransactionStatus
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/3/21 14:39:52
*描述：
*=====================================================================
*修改时间：2019/3/21 14:39:52
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace SAEA.DSModel
{
    public enum TransactionStatus
    {
        None = 0,
        Try = 1,
        Confirm = 2,
        Cancel = 3
    }
}
