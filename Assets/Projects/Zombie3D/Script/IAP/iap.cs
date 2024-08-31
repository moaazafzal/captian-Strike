using System.Runtime.InteropServices;
using UnityEngine;
public class IAP
{

    //unity ���� obj-c �Ľӿ�;
    [DllImport("__Internal")]
    protected static extern void PurchaseProduct(string productId, string productCount);
    public static void ToPurchaseProduct(string productId, string productCount)
    {
        Debug.Log("ToPurchaseProduct");
        PurchaseProduct(productId, productCount);
    }


    //������;
    //���磺Utils.NowPurchaseProduct(com.trinitigame.touchfish.099cents);
    public static void NowPurchaseProduct(string productId, string productCount)
    {
        Debug.Log("NowPurchaseProduct");
#if UNITY_IPHONE
        Debug.Log("NowPurchaseProduct iphone");
		ToPurchaseProduct(productId, productCount);
#endif
    }


    //�ص���������ⷢ�͹���Э��֮��Ĺ�����;
    public static int purchaseStatus(object stateInfo)
    {
        
        int pstatus = GetPurchaseStatus();

        return pstatus;

        //���洦����ɹ�������;
        //�����Ǯ;
    }

    [DllImport("__Internal")]
    protected static extern int PurchaseStatus();
    public static int OnPurchaseStatus()
    {
        //Debug.Log("OnPurchaseStatus");
        return PurchaseStatus();
    }


    public static int GetPurchaseStatus()
    {
        //Debug.Log("GetPurchaseStatus()");
#if UNITY_IPHONE
       
        //GameApp.GetInstance().GetGameState().DeliverIAPItem(itemList[i][currentScroll[i]].Name);
        int statusCode = OnPurchaseStatus();
        //Debug.Log("GetPurchaseStatus() iphone  " + statusCode);
		return statusCode;
#else
        return 1;
#endif
    }
}