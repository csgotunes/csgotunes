# This connection string is the default one for Azurite, which is what we use to emulate Azure Storage locally for development
$StorageContext = New-AzStorageContext -ConnectionString "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://storage:10000/devstoreaccount1;TableEndpoint=http://storage:10002/devstoreaccount1;QueueEndpoint=http://storage:10001/devstoreaccount1;"
New-AzStorageTable -Name "users" -Context $StorageContext
New-AzStorageTable -Name "nonces" -Context $StorageContext
New-AzStorageTable -Name "sessions" -Context $StorageContext
New-AzStorageQueue –Name "game-state" -Context $storageContext