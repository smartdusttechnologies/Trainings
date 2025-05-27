
$cert = New-SelfSignedCertificate -DnsName "localhost","basket-service","ocelotgateway","catalog.api","basket.api","ordering.api" `
  -CertStoreLocation "cert:\LocalMachine\My" -NotAfter (Get-Date).AddYears(1)

$pwd = ConvertTo-SecureString -String "1234" -Force -AsPlainText

Export-PfxCertificate -Cert $cert -FilePath "Certs/devcert.pfx" -Password $pwd

Export-Certificate -Cert $cert -FilePath "Certs/devcert.crt"
