
//obj-c �ĵ��ú���; plutin.mm
extern "C" void PurchaseProduct(const char* productId, const char* pCount)
{
	[ [MKStoreManager sharedManager] buyFeature:[ [NSString alloc] initWithCString:productId] productCount:[ [NSString alloc] initWithCString:pCount]];
}
extern "C" int PurchaseStatus()
{	
	return (int)[ [MKStoreManager sharedManager] purchaseStatus];
}