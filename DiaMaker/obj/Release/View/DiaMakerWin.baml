   M S B A M L   `   `   `  ���� C  ?DiaMaker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:DiaMaker.ViewDiaMaker.View  I)clr-namespace:Common.CommonLib.InterfacesCommon.CommonLib.Interfaces  L HDiaMakerViewModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullP:clr-namespace:DiaMakerViewModel;assembly=DiaMakerViewModelDiaMakerViewModel H DDiaMakerModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullD2clr-namespace:DiaMakerModel;assembly=DiaMakerModelDiaMakerModel     DiaMaker.View.DiaMakerWin   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -localclr-namespace:DiaMaker.View     7c)clr-namespace:Common.CommonLib.Interfaces     Hd:clr-namespace:DiaMakerViewModel;assembly=DiaMakerViewModel     @m2clr-namespace:DiaMakerModel;assembly=DiaMakerModel     -   $��_window��  � Title$  DiaMakerWin��$	��600��$
��1300�� � WindowState$ 	Maximized=� � WindowStartupLocation$ CenterScreen=� � 	Resources '   !DiaMaker.View.VisibilityConverter   visibilityConverter  ProxyElement � DataContext �� DataType.  (DiaMakerViewModel.RectangleInfoViewModel � 	Resources �� 
TargetType �� Property	 �� Value
 �� Left �� Path
 �� Top0  *DiaMakerViewModel.ConnectLineInfoViewModel � Points*  $DiaMakerViewModel.ArrowInfoViewModel � Points �� Color��%�  &	        &	      (��      *, +)(�� �    *, +)(�� T    *, +)'�� �    '�� |    '�     '�� C    '�� j    '�� �    '�� �      �  �� �� "   ���� '��       " ��.���  � ��$	 15(���  �@��TextAlignment$	 Center=���  � ��$	 Center=�.R��� -   ���#DD00AA$��0��$
 0��$
��Left=�.+��� -   
���   ����� *RectangleFill+���� *StrokeThickness+
 �� $
 Left� �� $	 Top����� $ Height����� $ Width��� $��3,2,2,7q����� $
 Name�
 �� $
 Left� �� $	 Top����� $ Height����� $ Width��� "  .R��� ���#DD00AA$��0��$
 0��$
��Left=�.+�� 
���   �$��2�� �� $ Points��� "  .R��� ���#DD00AA$��0��$
 0��$
��Left=�.+�� 
���   �$��1�� �� $ Points����� $ Black���� " ��.���  � ��$	 Stretch=���  � ��$	 Center=���  � ��$	 16(���  �@��Padding$	 5q���  � ��$	 5q���  � ��	 �#F8F8F8�� " ��.���  � ��$	 Stretch=���  � ��$	 Center=���  � ��$	 14(���  � ��$	 5q���  � ��$	 5q���  � ��	 �#E8E8E8�� " �.���  � ��	 �#EEEEEE�� " ��.���  � ��$	 Center=��� " ��.���  � ��$	 30����  � ��$	 5q���  � ��$	 5q��� " ��.���  � ��	 �#FAFAFA�� " ��.���  � ��$	 Stretch=���  � ��$	 16(���  � ��$	 2q���  � ��$	 5q���  � ��$	 Center=�.���  � RowDefinitions �� $
��Auto ��� $
��Auto ��� .+�� $��0�� � ColumnDefinitions �� $
��Auto ��� �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��0��$��0��$��Connection string���� $��0��$��1������ *ConnectionString+ �� Mode$ TwoWay=��� -    ��Name$ btnGetDBList��$��0��$��2��$��5q� � ToolTip$ Get DB list������ *GetDataBasesCommand+.���� $*��$picture\Addnewdatasource_8453_32.bmp���� $��3��$��5q�$ Create Diagramm������ *CreateDiaCommand+.���� $&�� picture\CreateSchema_8259_32.bmp���� $��4��$��5q�$ Previous variant������ *PreviousOptCommand+.���� $/��)picture\GotoPreviousUncovered_8603_32.bmp���� $��5��$��5q�$ Next variant������ *NextOptCommand+.���� $+��%picture\GoToNextUncovered_8593_32.bmp��� $��1�� �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� .+��� $��0��$��	Algorythm���� $��1��$��Center=����� *VerteciesInTableAlgorythms+ �� SelectedItem �� *#!SelectedVerteciesInTableAlgorythm+ %  DiaMakerViewModel.AlgorythmInfo��%�   (��       *, +)'�� 9     �� "  .R��� $��Center=�$��3q����� *Name+�� " ��.���  � ��$	 Center=���  � ��$	 Center=��� $��2��$��OptimalNumber���� $��4��$��Position in List���� $��6��$��Current Value���� $��3��$	��100������ *ChangeOptimalNumber+�� $��5��$	��100������ *PositionInBestRoutes+�� $��7��$	��100������ *WeightOfCurrentRoute+� $��2�� �� $	��300 ��� $
��Auto ��� $��* �.+�� $��0�� �� $
��Auto ��� $
��Auto ��� .+�� $��0�� �� $
��173* ��� $	��54* ��� $
��Auto �.+��� $��0��$��2������ *	DataBases+ �� SelectedIndex �� *SelectedDataBaseIndex+   DiaMakerModel.DataBase��%�   (��       *, +)'�� 9     �� "  .R��� $��Center=�$��3q����� *Name+�� " ��.���  � ��$	 Center=���  � ��$	 Center=��� $��2��$��5q�$��	Get table��$y�	Collapsed=����� *GetTablesCommand+� $��1�� �� $
��Auto ��� $
��Auto ��� .+��� $��1��`� $��0�� `� 
Background
 �����$��Center=�.�c�  c� Band$ 1�� c� 	BandIndex$ 1��$��Stretch=�.@��� $ 
Select All������ *SelectAllCommand+.���� $��25��$��25��$2��,Picture\Selectallrowsfrom_tableA__294_32.bmp���� $ UnSelect All������ *UnSelectAllCommand+.���� $��25��$��25��$$��Picture\UnComment_11701_32.bmp���� $" Select Current and Adjacents������ *" SelectCurrentAndAdjacentsCommand+.���� $��25��$��25��$2��,Picture\Selectallrowsfrom_tableA__294_32.bmp���� $# Select Adjacents For Selected������ *#!SelectAdjacentsForSelectedCommand+.���� $��25��$��25��$2��,Picture\Selectallrowsfrom_tableA__294_32.bmp���� $ Select Group������ *SelectGroupOfSelectedCommand+.���� $��25��$��25��$2��,Picture\Selectallrowsfrom_tableA__294_32.bmp���� $ Cancel Coloring������ *CancalColoringCommand+.���� $��25��$��25��$2��,Picture\Selectallrowsfrom_tableA__294_32.bmp���� $��2��$y�	Collapsed=�#��[ &   System.Windows.Controls.DataGrid  $��2��   
IsReadOnly .     AutoGenerateColumns .  $��7,13,7,1q� �� AlternationCount$ 2��   RowBackground �#DDDDFF   AlternatingRowBackground �#CCCCEE   SelectionMode$ Single=�    SelectionUnit$  FullRow=�!   CanUserAddRows! .  "   CanUserDeleteRows" .  ���� *Tables+ �� *SelectedTable+#   RowStyle# �� )  #System.Windows.Controls.DataGridRow"  .���  � ��$	 25����  � ��$	 Center=���  � ��$	 Center=�$   	CellStyle$ �� *	  $System.Windows.Controls.DataGridCell" 	 .���  � ��$	 Center=���  � ��$	 Center=�%   Columns% 0
  *System.Windows.Controls.DataGridTextColumn
  ,  &System.Windows.Controls.DataGridColumn&   Header$
& Name��'   MinWidth$	' 200Q�1  +System.Windows.Controls.DataGridBoundColumn(   Binding( �� *Name+4  .System.Windows.Controls.DataGridCheckBoxColumn  $& Is selected��)   Width5  /System.Windows.Controls.DataGridLengthConverter$
) Auto *   MaxWidth$* 30Q�( �� $ 
IsSelected�+ �� UpdateSourceTrigger$+ PropertyChanged=�4  .System.Windows.Controls.DataGridTemplateColumn  $& Gr��$' 10Q�,   
Visibility, �� *DataContext.IsColored+- �� Source#- [ . �� 	Converter#. [  /   CellTemplate/ �� .R��� $��Stretch=�0 �� Padding$0 5,0,0,0q����� *Group+���� *
TableColor+���� *	ForeColor+�� $��1��$��1��$
��Left=�$��Stretch=�
���   �1 �� ShowsPreview1 . $��3��� $��2�����#E9FAFB.+��� -   2 ��Name$2 pnlDia��
���   �$��2q�._��� .���� -   3 ��Name$3 icDia��$��5q����� *DiaElements+���� 4 �� 
TargetType"4 ��.R��� ���	#00000000$��1q�$
��Left=�._��� ���� .R�� ���	#00000000