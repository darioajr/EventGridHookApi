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
```