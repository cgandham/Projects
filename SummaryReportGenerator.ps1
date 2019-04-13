$date= (Get-Date).ToString('dd-MM-yyyy')
# $k = ("OverAll") 
$k = ("Planning")
$bch = "Branch Coverage"
$module = "Module Count"
$sequential = "Sequential Point Coverage"
$report = "Detailed Report Link"
$destDir = "C:\NCoverReports\$date"
If (!(Test-Path $destdir)) {
   New-Item -Path $destDir -ItemType Directory
}


$strFile= "C:\NCoverjobs\$filename"		#For Filename
if (!(Test-Path "\\hyd-qatools\NCover\Latest"))           #create file if it does not exist
{
   New-Item -path "\\hyd-qatools\NCover\Latest" -type "directory"
}

if (!(Test-Path "\\hyd-qatools\NCover\Archive"))           #create file if it does not exist
{
   New-Item -path "\\hyd-qatools\NCover\Archive" -type "directory"
}

Move-Item -force \\hyd-qatools\NCover\Latest\*.html \\hyd-qatools\NCover\Archive

if (!(Test-Path "$destDir\a.txt"))           #create file if it does not exist
{
   New-Item -path "$destDir\a.txt" -type "file"
}
if (!(Test-Path "$destDir\b.txt"))		#create file if it does not exist
{
   New-Item -path "$destDir\b.txt" -type "file"
}
if (!(Test-Path "$destDir\c.txt"))		#create file if it does not exist
{
   New-Item -path "$destDir\c.txt" -type "file"
}
if (!(Test-Path "$destDir\ht.txt"))           #create file if it does not exist
{
   New-Item -path "$destDir\ht.txt" -type "file"
}

$a = "<style>"						#for styling table
 
$a = $a + "BODY{background-color:White;    width: 775px;
    margin: 10px auto;
    min-height: 800px;
	position: relative}"
$a = $a + "TABLE{border-width: 1px;border-style: solid;border-color: rgba(100,100,100,0.5);border-collapse: collapse;}"
$a = $a + "TH{border-width: 1px;padding: 8px;border-style: solid;border-color: rgba(100,100,100,0.5);background-color:rgba(135, 135, 144, 0.3);
	  font-family:Helvetica,Arial,sans-serif !important;color:rgba(10, 10, 10, 0.7);line-height:20px;
	  text-align:center;font-weight:bold;padding-top:3px;font-size:small}"

$a = $a + "TD{border-width: 1px;padding: 8px;border-style: solid;border-color: rgba(100,100,100,0.5);background-color:rgba(235, 235, 242, 0.5);
	   font-family:Helvetica,Arial,sans-serif !important;
	   font-size: 12px;color:rgba(10, 10, 10, 0.8);line-height:20px;text-align:center}"

$a = $a + "</style>"

	

NCover.exe report --project="Planning" --filter="Planning Post Coverage" --file="$destDir\Planning_$date"





Copy-Item -path "$destDir\*.html" -destination  "\\hyd-qatools\NCover"

#NCover Summarize --project="Consolidation" --execution="NightlyBuild_Consolidation"| Out-File "$destDir\Summary.txt"
#$p= Select-String -Path "$destDir\Summary.txt" -Pattern 'Start Time'| Out-File "$destDir\temp.txt"
#$q= Get-Content -path "$destDir\temp.txt" | %{ [Regex]::Matches($_, '[0-9]*/[0-9]*/[0-9]*') } | %{ $_.Value}|select-object -first 1

$branch = @()   #array to store all branch cvg values
$seq = @()	#array to store all seq points values
$mod = @()	#array to store all module count values

for ($j = 0; $j -lt  $k.count; $j++)
{

#$filename = $k[$j] +'_'+ $date
$filename =  "Planning_$date"
#$somePath = "\Users\cgandham\Documents\"
#$filename =  "Reporting_current_Summary"


#for getting branch cvg values from each report
$g= Select-String -path $destDir\$filename.html -Pattern '"coverageType": "BP"' -Context 0,2 | Out-File $destDir\a.txt

$b= Get-Content -path $destDir\a.txt | %{ [Regex]::Matches($_, '[0-9]*.[0-9]*%') } | %{ $_.Value} | select-object -first 1
$branch += $b
#for getting seq points from each report
$h= Select-String -path $destDir\$filename.html -Pattern '"coverageType": "SP"' -Context 0,2 | Out-File $destDir\b.txt
$s= Get-Content -path $destDir\b.txt | %{ [Regex]::Matches($_, '[0-9]*.[0-9]*%') } | %{ $_.Value}|select-object -first 1
$seq += $s
#for getting module counts
$m= Get-Content -path $destDir\$filename.html | %{ [Regex]::Matches($_, '"moduleCount": [0-9]*') } | %{ $_.Value} | Out-File $destDir\c.txt
$mc= Get-Content -path $destDir\c.txt | %{ [Regex]::Matches($_, '[0-9]+') } | %{ $_.Value} | select-object -first 1
$mod += $mc
}
$MyProperties = @{}		#array of objects
$htmlobj = @{}          #array of existing table objects
$tobj = @{}             #array of total object
$myArray = @()			#array to store objects
for ($j = 0; $j -lt  $k.count; $j++)
{
#$filename = $k[$j] +'_'+ $date
$filename =  $date
#$strFile= "<a  href='/AutomationSite/Ncover/Latest/$filename.html'>Report Link</a>"	

$myProperties[$j] = New-Object System.Object						#new file
$myProperties[$j] | Add-Member -type NoteProperty -name Area -Value "Planning"
$myProperties[$j] | Add-Member -type NoteProperty -name $bch -Value $branch[$j]
$myProperties[$j] | Add-Member -type NoteProperty -name $sequential -Value $seq[$j]
$myProperties[$j] | Add-Member -type NoteProperty -name $module -Value $mod[$j]
#$myProperties[$j] | Add-Member -type NoteProperty -name $report -Value  $strFile	
$myArray += $myProperties[$j]
} 

$htmlpath = "\\hyd-qatools\NCover\$date.html"   
if (Test-Path $htmlpath)
 {
 $html = Get-Content "\\hyd-qatools\NCover\$date.html" 
 [regex]::Matches($html, '<tr.*?>(.+)</tr>') | % {$_.Captures[0].Groups[1].value} | Out-File $destDir\ht.txt
$b = Get-Content -path  $destDir\ht.txt | %{ [Regex]::Matches($_, '(?<=<td>)(.*?)(?=</td>)') } | %{ $_.Value} 

$htmldata = @()	
$brhtml = @()	
$sqhtml = @()	
$modhtml = @()	

for ($j = 0; $j -lt  $b.count - 4; $j++)
{
$htmldata += $b[$j] #Remove total row
}

$arhtml = [regex]::Matches($htmldata, '\b[^\d\W]+\b')     #All areas from table
$sh=2
$mh=3
for($bh=1 ; $bh -lt $htmldata.length; $bh+=4)
{
$brhtml += $htmldata[$bh]       #Add all branch values from table
$sqhtml += $htmldata[$sh]       #Add all seq values from table
$modhtml += $htmldata[$mh]      #Add all mod values from table
$sh+=4 
$mh+=4
}
for ($h = 0; $h -lt  $arhtml.count; $h++)
{
 $htmlobj[$h] = New-Object System.Object						#new file
 $htmlobj[$h] | Add-Member -type NoteProperty -name Area -Value $arhtml[$h]
 $htmlobj[$h] | Add-Member -type NoteProperty -name $bch -Value $brhtml[$h]
 $htmlobj[$h] | Add-Member -type NoteProperty -name $sequential -Value $sqhtml[$h]
 $htmlobj[$h] | Add-Member -type NoteProperty -name $module -Value $modhtml[$h] 
 $myArray += $htmlobj[$h]
}
for ($t = 0; $t -lt  $arhtml.count; $t++)  #add html values 
{
$branch += $brhtml[$t]   
$seq += $sqhtml[$t]
$mod += $modhtml[$t]
}
} 


$br = @() #branch array replacing %
$sq = @() #sequential array replacing %
[string]$pr = "%"
foreach($b in $branch)
{
   $b = $b -replace '"',' ' -replace '%',' '
   $br += $b  #adding replaced data
}
foreach($s in $seq)
{
   $s = $s -replace '"',' ' -replace '%',' '
    $sq += $s  #adding replaced data
}
 $c = $myArray.count
 
 $sum = $br -join '+'
 $brtotal = Invoke-Expression $sum 
 $brtotal /= $c
 [string]$branchtotal = $brtotal
 $branchtotal = $branchtotal + $pr
 
 
 $sum1 = $sq -join '+'
 $sqtotal =  Invoke-Expression $sum1 
 $sqtotal /= $c
 [string]$seqtotal = $sqtotal
 $seqtotal = $seqtotal + $pr
 
 $sum2 = $mod -join '+'
 $modtotal = Invoke-Expression $sum2
 

 $i = 0 
 $tobj[$i] = New-Object System.Object						#new file
 $tobj[$i] | Add-Member -type NoteProperty -name Area -Value "TOTAL"
 $tobj[$i] | Add-Member -type NoteProperty -name $bch -Value $branchtotal
 $tobj[$i] | Add-Member -type NoteProperty -name $sequential -Value $seqtotal
 $tobj[$i] | Add-Member -type NoteProperty -name $module -Value $modtotal 
 $myArray += $tobj[$i]

$pso = $myArray | ConvertTo-Html -head $a #-PreContent “<h4>Build Id: NightlyBuild_$date</h4><h4>Execution Date: $q</h4>”
$pso -replace '&gt;','>'  -replace '&lt;','<' -replace '&#39;',"'" | Out-File \\hyd-qatools\NCover\Latest\Latest_Report.html
#$pso -replace '&gt;','>'  -replace '&lt;','<' -replace '&#39;',"'" | Out-File $destdir\$date.html
$pso -replace '&gt;','>'  -replace '&lt;','<' -replace '&#39;',"'" | Out-File  \\hyd-qatools\NCover\$date.html



Remove-item -path "$destDir\a.txt"
Remove-item -path "$destDir\b.txt"
Remove-item -path "$destDir\c.txt"
Remove-item -path "$destDir\ht.txt"
# Remove-item -path "$destDir\temp.txt"
# Remove-item -path "$destDir\Summary.txt"