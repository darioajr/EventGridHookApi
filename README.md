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
