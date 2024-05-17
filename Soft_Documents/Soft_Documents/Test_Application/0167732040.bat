@ECHO.
@ECHO.
@ECHO. *****************************************************************************
@ECHO. ***                          ROBIN_BEST_TOUCH          			 ***
@ECHO. ***                     PROGRAMLAMASI BASLATILIYOR                        ***
@ECHO. ***                                                                       ***
@ECHO. *****************************************************************************

@ECHO OFF
echo Starting Programming Application
cd "C:\Users\serkan.baki\Desktop\Soft_Documents\Test_Application\HEX-file"
dir /b /a-d > out.tmp
set /p hexname=< out.tmp
del out.tmp
cd ..
echo.
echo.
echo Hex File name picked from the folder is: %hexname%
echo.
@ECHO. -----------------------------------------------------------------------------
"C:\Users\serkan.baki\Desktop\Soft_Documents\Test_Application\PSoC_4_Programmer.exe" %hexname%

