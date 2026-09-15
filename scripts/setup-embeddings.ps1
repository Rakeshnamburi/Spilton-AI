$ErrorActionPreference='Stop'
$projectRoot=Split-Path $PSScriptRoot -Parent
$target=Join-Path $projectRoot '.local/models/minilm'
New-Item -ItemType Directory -Force $target | Out-Null
$revision='1110a243fdf4706b3f48f1d95db1a4f5529b4d41'
$base="https://huggingface.co/sentence-transformers/all-MiniLM-L6-v2/resolve/$revision"
$hashes=@{'model.onnx'='6FD5D72FE4589F189F8EBC006442DBB529BB7CE38F8082112682524616046452';'vocab.txt'='07ECED375CEC144D27C900241F3E339478DEC958F92FDDBC551F295C992038A3'}
# Approximately 90.4 MB total model download. CPU only; no GPU driver or Python installation.
foreach($file in @('model.onnx','vocab.txt')) {
    $destination=Join-Path $target $file
    if(Test-Path -LiteralPath $destination){if((Get-FileHash -LiteralPath $destination).Hash -ne $hashes[$file]){throw "Unexpected hash for existing $file. No file was replaced."};Write-Host "$file already exists and its hash matches; keeping it.";continue}
    $remote=if($file -eq 'model.onnx'){'onnx/model.onnx'}else{$file}
    Invoke-WebRequest "$base/$remote" -OutFile "$destination.download"
    if((Get-FileHash -LiteralPath "$destination.download").Hash -ne $hashes[$file]){throw "Downloaded $file failed its checksum."}
    Move-Item -LiteralPath "$destination.download" -Destination $destination
}
Write-Host 'Local MiniLM files ready. Run the embedding tests to verify inference before ingestion.'
