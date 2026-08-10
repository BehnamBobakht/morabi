using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enumarables
{
    public enum CartUpdateType 
    {
        None,
        Add,
        Increase,
        Decrease,
        Delete,
        SetAddress,
        SetRebate,
        SetDelivery,
        SetMerchant,
        SetAccountId,
        SetOther

    }   
    public enum CartAddressType : byte
    {
        Select,
        Add,
        Delete,      
    }  
    public enum CartHeader : byte
    {
        Index,
        Address,
        Confirm,      
        CallBack,      
    }
}
