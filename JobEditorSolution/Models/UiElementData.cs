using Communication.Plc;
using Communication.Plc.Shared;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Models
{
    [Serializable]
    public class UiElementData<TValue> : IPlcMappable, ITranslatable
    {
        public uint BackGround
        {
            get;
            set;
        }

        public bool Blink
        {
            get;
            set;
        }

        public int ContentData
        {
            get;
            set;
        }

        public string ContentImage
        {
            get;
            set;
        }

        public string ContentText
        {
            get;
            set;
        }

        public bool EventState
        {
            get;
            set;
        }

        public UiElementDataEventType EventType
        {
            get;
            set;
        }

        public UiElementDataFormat Format
        {
            get;
            set;
        }

        public bool IsEnabled
        {
            get;
            set;
        }

        public int Localisation
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string TextId
        {
            get;
            set;
        }

        public Type Type
        {
            get;
            set;
        }

        public TValue Value
        {
            get;
            set;
        }

        public int ValueUpdateCounter
        {
            get;
            set;
        }

        public UiElementDataVisibility Visibility
        {
            get;
            set;
        }

        public UiElementData()
        {
            this.Visibility = UiElementDataVisibility.Visible;
            this.IsEnabled = true;
            this.EventType = UiElementDataEventType.None;
            this.EventState = false;
            this.Localisation = 0;
            this.Format = UiElementDataFormat.Default;
            this.Value = default(TValue);
            this.ValueUpdateCounter = 0;
            this.BackGround = 0;
            this.Blink = false;
        }

        public UiElementData(UiElementData<TValue> dataTemplate)
        {
            this.ContentText = dataTemplate.ContentText;
            this.ContentImage = dataTemplate.ContentImage;
            this.ContentData = dataTemplate.ContentData;
            this.BackGround = dataTemplate.BackGround;
            this.Blink = dataTemplate.Blink;
            this.Visibility = dataTemplate.Visibility;
            this.IsEnabled = dataTemplate.IsEnabled;
            this.EventType = dataTemplate.EventType;
            this.EventState = dataTemplate.EventState;
            this.Localisation = dataTemplate.Localisation;
            this.Format = dataTemplate.Format;
            this.Value = dataTemplate.Value;
            this.ValueUpdateCounter = 0;
        }

        public string Map(List<byte> dataBlock)
        {
            TypeCode typeCode = Type.GetTypeCode(typeof(TValue));
            string str = "T_UiControlData";
            PlcControlDataRaw flag = new PlcControlDataRaw()
            {
                ContentData = (short)this.ContentData,
                ContentImage = this.ContentImage,
                ContentText = this.ContentText,
                TextId = this.TextId,
                EventState = this.EventState,
                BackGround = this.BackGround,
                Blink = this.Blink,
                EventType = (short)this.EventType,
                Format = (short)this.Format,
                IsEnabled = this.IsEnabled,
                Localisation = (short)this.Localisation,
                Visibility = (short)this.Visibility
            };
            if (typeCode <= TypeCode.UInt64)
            {
                if (typeCode == TypeCode.Boolean)
                {
                    flag.Type = 3;
                    flag.BoolValue = Convert.ToBoolean(this.Value);
                }
                else if (typeCode == TypeCode.UInt64)
                {
                    flag.Type = 2;
                    flag.IntegerValue = Convert.ToUInt64(this.Value);
                }
            }
            else if (typeCode == TypeCode.Double)
            {
                flag.Type = 1;
                flag.RealValue = Convert.ToDouble(this.Value);
            }
            else if (typeCode == TypeCode.String)
            {
                flag.Type = 0;
                flag.StringValue = Convert.ToString(this.Value);
            }
            dataBlock.AddRange(PlcMapper.GetBytes(flag));
            return str;
        }

        public bool Map(object rawType)
        {
            if (!(rawType is PlcControlDataRaw))
            {
                return false;
            }
            PlcControlDataRaw plcControlDataRaw = (PlcControlDataRaw)rawType;
            UiElementDataType type = (UiElementDataType)plcControlDataRaw.Type;
            this.ContentData = plcControlDataRaw.ContentData;
            this.ContentImage = plcControlDataRaw.ContentImage;
            this.BackGround = plcControlDataRaw.BackGround;
            this.Blink = plcControlDataRaw.Blink;
            this.TextId = plcControlDataRaw.TextId;
            this.ContentText = plcControlDataRaw.ContentText;
            this.EventState = plcControlDataRaw.EventState;
            this.EventType = (UiElementDataEventType)plcControlDataRaw.EventType;
            this.Format = (UiElementDataFormat)plcControlDataRaw.Format;
            this.IsEnabled = plcControlDataRaw.IsEnabled;
            this.Localisation = plcControlDataRaw.Localisation;
            this.Visibility = (UiElementDataVisibility)plcControlDataRaw.Visibility;
            TypeCode typeCode = Type.GetTypeCode(typeof(TValue));
            if (typeCode > TypeCode.UInt64)
            {
                if (typeCode == TypeCode.Double)
                {
                    if (type != UiElementDataType.LReal)
                    {
                        return false;
                    }
                    if (!EqualityComparer<TValue>.Default.Equals((TValue)(object)plcControlDataRaw.RealValue, this.Value))
                    {
                        this.ValueUpdateCounter = this.ValueUpdateCounter + 1;
                    }
                    this.Value = (TValue)(object)plcControlDataRaw.RealValue;
                    return true;
                }
                if (typeCode == TypeCode.String)
                {
                    if (type != UiElementDataType.String)
                    {
                        return false;
                    }
                    if (!EqualityComparer<TValue>.Default.Equals((TValue)(object)plcControlDataRaw.StringValue, this.Value))
                    {
                        this.ValueUpdateCounter = this.ValueUpdateCounter + 1;
                    }
                    this.Value = (TValue)(object)plcControlDataRaw.StringValue;
                    return true;
                }
            }
            else
            {
                if (typeCode == TypeCode.Boolean)
                {
                    if (type != UiElementDataType.Bool)
                    {
                        return false;
                    }
                    if (!EqualityComparer<TValue>.Default.Equals((TValue)(object)plcControlDataRaw.BoolValue, this.Value))
                    {
                        this.ValueUpdateCounter = this.ValueUpdateCounter + 1;
                    }
                    this.Value = (TValue)(object)plcControlDataRaw.BoolValue;
                    return true;
                }
                if (typeCode == TypeCode.UInt64)
                {
                    if (type != UiElementDataType.LWord)
                    {
                        return false;
                    }
                    if (!EqualityComparer<TValue>.Default.Equals((TValue)(object)plcControlDataRaw.IntegerValue, this.Value))
                    {
                        this.ValueUpdateCounter = this.ValueUpdateCounter + 1;
                    }
                    this.Value = (TValue)(object)plcControlDataRaw.IntegerValue;
                    return true;
                }
            }
            return false;
        }
    }
}