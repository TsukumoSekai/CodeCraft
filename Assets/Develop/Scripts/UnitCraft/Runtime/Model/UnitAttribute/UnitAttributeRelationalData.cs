//using FrameCraft.Model;
//using FrameCraft.Utility;
//using NetNorth.Utility;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace NetNorth.Battle.Unit.Model
//{
//    [System.Serializable]
//    public class UnitAttributeRelationalData : BaseData
//    {
//        private static Dictionary<uint, UnitAttributeRelationalData> m_DataDict;

//        public new static Dictionary<uint, UnitAttributeRelationalData> DataDict
//        {
//            get
//            {
//                if (m_DataDict == null)
//                    m_DataDict = DatabaseUtility.LoadData<uint, UnitAttributeRelationalData>($"{DatabaseUtility.DatabasePath}/UnitAttributeRelationalDatabase");
//                return m_DataDict;
//            }
//        }

//        [ExcelElement("属性关系表")]
//        public string m_RelationalTableString
//        {
//            set
//            {
//                string[] elementArray = value.Split(ExcelUtility.SYMBOL_FIRST);

//                m_RelationalTableArray = new UnitAttributeRelationalTable[elementArray.Length];

//                for (int i = 0; i < elementArray.Length; i++)
//                {
//                    //m_RelationalTableArray[i] = (UnitAttributeRelationalTable)ScriptableObjectUtility.ParseDataMember(typeof(UnitAttributeRelationalTable), elementArray[i], ExcelUtility.SYMBOL_SECOND[0]);
//                    m_RelationalTableArray[i] = (UnitAttributeRelationalTable)DatabaseUtility.ParseDataMember(typeof(UnitAttributeRelationalTable), elementArray[i], ExcelUtility.SYMBOL_SECOND);
//                }

//            }
//        }

//        public UnitAttributeRelationalTable[] m_RelationalTableArray;
//    }

//    [System.Serializable]
//    public class UnitAttributeRelationalTable
//    {
//        [DataMemberIndex(0)]
//        public int RelationalAttributeTypeIndex
//        {
//            set => m_RelationalAttributeType = (UnitAttributeType)value;
//            get => (int)m_RelationalAttributeType;
//        }

//        [DataMemberIndex(1)]
//        public float m_RelationalAttributeBonus;

//        public UnitAttributeType m_RelationalAttributeType;
//    }
//}