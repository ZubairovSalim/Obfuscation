   M S B A M L   `   `   `  ����F  BObfuscation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:ObfuscationObfuscation  5   	       Obfuscation.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:Obfuscation     5   	     � Title$  
MainWindow��5   	   $	��630��6   $	��637��6)    � WindowStartupLocation$ CenterScreen=�65    � 
ResizeMode$ NoResize=�6Z   .��5	      �� .+�5
   
   �� -   5         ��Name$ btn_AddOriginalCode��5
      $
��Left=�6.   $��30��6I   $	��200��6U   $��0,10,0,0q�6a   .��6�   *(Добавить исходный код6�   �� 5   
    �� Orientation$ 
Horizontal=�6   $	��400��6.   $��	0,10,0,20q�6;   .+�5      �� -   5         ��Name$ txb_OriginalCode��5      $	��290��60   $��0,0,40,0q�6<    �� AcceptsReturn . " �� VerticalScrollBarVisibility$ Visible=�6c    �� TextWrapping$
 Wrap=�6�   �� 5      -   5        $ txb_ObfuscatedCode��5      $	��290��62    . $ Visible=�6S   $
 Wrap=�6y   6�   5      �� 5   
   $ 
Horizontal=�6   .+�5      �� -   5        $ 	btn_Save1��5      $��30��6(   $	��200��64   $��0,0,10,0q�6@   .��6i   ,*Сохранить исходный код6�   �� 5      -   5        $ btn_Obfuscate��5      $��30��6,   $	��200��68   $��0,0,10,0q�6D   .��6r   Обфусцировать6�   �� 5      -   5        $ 	btn_Save2��5      $��30��6(   $	��200��64   .��6W   :8Сохранить обфусцированный код6v   5      �� 5   
   $ 
Horizontal=�6   $��	0,10,0,10q�6.   .+�5      �� -   5        $ txb_OriginalPath��5      $��30��60   $	��280��6<   $��0,0,40,0q�6H   6\   �� 5      -   5        $ txb_ObfuscatedPath��5      $	��280��62   6@   5      �� 5   
   $ 
Horizontal=�6   .+�5      �� -	   5        $ btn_Run1��5      $��30��6'   $	��200��63   $��	95,0,10,0q�6?   .��6h   ,*Запустить исходный код6�   �� 5      -
   5        $ btn_Run2��5      $��30��6'   $	��200��63   .��6U   :8Запустить обфусцированный код6t   5      5      5      