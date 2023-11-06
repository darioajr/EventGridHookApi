# EventGridHookApi
Aspnet Core EventGridHookApi that process blob created notification

## Missing to do
- Create and destroy blob with terraform
- Create adn destroy Event Grid with terraform

## Inicializar Terraform
```bash
terraform init -upgrade
```

## Criar um plano de execução Terraform
```bash
terraform plan -out main.tfplan
```

## Aplicar um plano de execução do Terraform
```bash
terraform apply main.tfplan
```

## Verifique os resultados
```bash
terraform output -raw container_ipv4_address
```

## Limpar os recursos
```bash
terraform plan -destroy -out main.destroy.tfplan
terraform apply main.destroy.tfplan
```

## Criação do Hook do EventGrid
1. `retry_policy`: Define quantas vezes o Azure tentará entregar o evento em caso de falha. Neste exemplo, ele tentará 5 vezes.
2. `event_time_to_live`: Define quanto tempo o Azure manterá o evento para tentar entregá-lo. Aqui, é configurado para 2 horas.
3. `dead_letter_destination`: Se o Azure não conseguir entregar o evento após as tentativas configuradas, ele será enviado para um blob container de "dead letter". Aqui, configuramos um container chamado "deadletters" para armazenar esses eventos.
4. `Logs`: Os logs de entrega do Azure Event Grid podem ser encaminhados para diferentes destinos. Neste caso, estamos usando um blob container.

Lembre-se de que este é um exemplo básico. Em um ambiente de produção, você deve considerar outros aspectos, como segurança, monitoramento avançado e manuseio adequado de mensagens na API.

## Alternativa Comandos AZ

```bash
# Define your variables
resourceGroup="MyResourceGroup"
storageAccount="mystorageaccount$(date +%s)"
location="eastus"
containerName="mycontainer"
eventGridTopic="myeventgridtopic"
webhookEndpoint="http://localhost:8000/eventgridhook" # Substitute this with your actual endpoint
deadLetterStorageAccount="mydeadletteraccount$(date +%s)"
deadLetterContainer="deadlettercontainer"

# 1. Criar um grupo de recursos
az group create --name $resourceGroup --location $location

# 2. Criar uma conta de armazenamento e um contêiner blob
az storage account create --name $storageAccount --location $location --resource-group $resourceGroup --sku Standard_LRS
accountKey=$(az storage account keys list --account-name $storageAccount --resource-group $resourceGroup --query "[0].value" --output tsv)
az storage container create --account-name $storageAccount --name $containerName --account-key $accountKey

# 3. Registrar um provedor de recursos do Event Grid (se necessário)
az provider register --namespace Microsoft.EventGrid

# 4. Criar um tópico do Event Grid e inscrever um endpoint (webhook)
az eventgrid topic create --name $eventGridTopic --location $location --resource-group $resourceGroup
az eventgrid event-subscription create --resource-group $resourceGroup --topic-name $eventGridTopic --name "mysubscription" --endpoint $webhookEndpoint

# 5. Configurar a política de dead-letter e tentativas para a assinatura do Event Grid
# Criar uma conta de armazenamento para dead-letter
az storage account create --name $deadLetterStorageAccount --location $location --resource-group $resourceGroup --sku Standard_LRS
deadLetterAccountKey=$(az storage account keys list --account-name $deadLetterStorageAccount --resource-group $resourceGroup --query "[0].value" --output tsv)
az storage container create --account-name $deadLetterStorageAccount --name $deadLetterContainer --account-key $deadLetterAccountKey

# Atualizar a assinatura do Event Grid para incluir a política de dead-letter
az eventgrid event-subscription update --resource-group $resourceGroup --topic-name $eventGridTopic --name "mysubscription" --deadletter-endpoint "https://$deadLetterStorageAccount.blob.core.windows.net/$deadLetterContainer" --max-delivery-attempts 20 --event-ttl 1440

# Note: Para o Service Bus, você usaria um endpoint diferente e criaria uma fila ou tópico do Service Bus

```