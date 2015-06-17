//RETAINGLOBALVAR
CONTINUEONCANCEL

//versao 2.0.2
//data 26/05/2015

	//CONST
	var gbl_const_NFCE 							:A4 = "NFCE"
	var gbl_const_SAT 							:A3 = "SAT"
	
	//GBL Ambiente
	var gbl_IFC_Status							:N1
	var gbl_Check_Processed					:N1
	


	//GBL Configs
	var gbl_Conf_IP_Print						:A20
	var gbl_Conf_IP_Send 						:A20
	var gbl_Conf_Porta_Send					:N5	
	var gbl_Conf_ImprimirDanfe			:N1 //0 NOT | 1 Yes	
	var gbl_Conf_TefStart						:N9
	var gbl_Conf_TefEnd							:N9
	
	var gbl_Conf_TefStartDEB				:N9
	var gbl_Conf_TefEndDEB					:N9
	
	var gbl_Conf_TefStartCRED				:N9
	var gbl_Conf_TefEndCRED					:N9
	
	var gbl_Conf_Tef_Server					:A20
	var gbl_Conf_Tef_Loja						:A20	
	var gbl_Conf_Tef_Enable					:N1
	
	//Inq Room Charge
	var gbl_Conf_PagHotelaria				:N9
	
	//Inq Passantes
	var gbl_Conf_PagHotelaria51				:N9
	var gbl_Conf_PagHotelaria52				:N9
	var gbl_Conf_PagHotelaria53				:N9
	var gbl_Conf_PagHotelaria54				:N9
	var gbl_Conf_PagHotelaria55				:N9
	var gbl_Conf_PagHotelaria56				:N9
	var gbl_Conf_PagHotelaria57				:N9
	var gbl_Conf_PagHotelaria58				:N9
	var gbl_Conf_PagHotelaria59				:N9
	var gbl_Conf_PagHotelaria60				:N9
	var gbl_Conf_PagHotelaria61				:N9
	var gbl_Conf_PagHotelaria62				:N9
	var gbl_Conf_PagHotelaria63				:N9
	var gbl_Conf_PagHotelaria64				:N9
	var gbl_Conf_PagHotelaria65				:N9
	var gbl_Conf_PagHotelaria66				:N9
	var gbl_Conf_PagHotelaria67				:N9
	var gbl_Conf_PagHotelaria68				:N9
	var gbl_Conf_PagHotelaria69				:N9
	var gbl_Conf_PagHotelaria70				:N9
	
	var gbl_Conf_SAT_NFCE							:A10
	
	var gbl_Conf_DadosPagante				:N1

	//
	var hDLLEnvio :N10
	var hDLLEpson :N10
	var hDLLSitef	:N10	
	var hDLLCmd		:N10 // "\micros\res\pos\etc\pccmd.dll"

	
	var gbl_dllEpsonImpressao :A50 = "InterfaceEpsonNF.dll"
	
	//globals para envio	
	var gbl_envio_header [50]:A1000 	
	var gbl_envio_itens [200]:A1000
	var gbl_envio_payments [50]:A1000
	
	var gbl_envio_dados[999]:A1000 //Array geral para envio a DLL Cliente SAT
	var gbl_envia_dados_msg :A20000
	
	var gbl_envia_dados_retorno :A20000
	
	var gbl_envia_dados_QrCode :A20000
	var gbl_envia_dados_BaCode :A20000
	
	var gbl_envio_header_count :N9
	var gbl_envio_itens_count :N9
	var gbl_envio_payments_count :N9

	//globais para configuracao
	var gbl_config_LOG_INTERFACE 						:N1 = 1
	var gbl_config_CapturaDadosCliente			:N1 = 1	
	var gbl_config_DadosCompletosdoCliente	:N1 = 1
	
	//globais para Tipos
	var cTrue 										:N1= 1
	var cFalse 										:N1= 0		
	
	//globais Outros
	var gbl_LogText								:A20000
	
	VAR PCWS : N1 = 1
	VAR HHT : N1 = 2
	VAR WS4 : N1 = 3
		
	VAR gbl_WndColSize[3] : N2	
		gbl_WndColSize[PCWS] = 65
		gbl_WndColSize[WS4] = 65
		gbl_WndColSize[HHT] = 34	
	
	VAR gbl_TamMinimo :N5
	VAR gbl_TamMaximo :N5
	VAR gbl_MenuTitle :A255
	
	var tmpString :A10000
		
	// TEF
	VAR gbl_TefPayment :N1 // 0 = Nao | 1 = Sim
	VAR OldPromptMessage :A1024
	VAR Buffer : A20000
	VAR Comando : A12
	VAR TipoCampo : A12
	VAR MenuTitle : A255
	VAR gbl_TEF_TamMinimo : A6
	VAR gbl_TEF_TamMaximo : A6
	VAR gbl_TEF_Validation :N03
	
	VAR PromptForContinuePeripheralsInput :N01
	
	VAR field_100 : A10
	VAR field_131 : A10
	VAR field_132 : A10
	
	VAR field_100_idx[20] : A10
	VAR field_100_val[20] : A10
	VAR countfield100 : N03
	VAR field_132_idx[50] : A10
	VAR field_132_val[50] : A10
	VAR countfield132 : N03
	VAR TEFCupom : A11 = " "
	VAR TEFTMedInfo : A100
	VAR TEFNSUinfo : A24
	VAR TEFtmed_ttl :$12
	VAR TEFtmdnum : N9
	
	VAR TEFVoucherBuffer1[800] : A48 // printer limit = 618
	VAR TEFVoucher1LineCnt : N07
	VAR TEFVoucherBuffer2[800] : A48 // printer limit = 618
	VAR TEFVoucher2LineCnt : N07	
	
	//GBL para desconto por Item
	VAR gbl_Desc_Position[999]:N9	
	
	//gbl Linhas de rodape
	var gbl_Linha9				:A48
	var gbl_Linha8				:A48
	var gbl_Linha7				:A48
	var gbl_Linha6				:A48
	var gbl_Linha5				:A48
	var gbl_Linha4				:A48
	var gbl_Linha3				:A48
	var gbl_Linha2				:A48
	var gbl_Linha1				:A48

	//global controle de tentativas
	var gbl_try_and_retry	:n1
	
	var const_NFCE_OK :A6 = "NFCeOK"

event inq : 3 //TEF By INQ
	
	VAR payment : $12
	VAR keypress_confirm : key
	VAR tndnum_ : N9 = 0
	VAR do_tef_ : N1 = cFALSE
	VAR CKID_H : A1
	VAR keypress_confirm2 : key
	VAR tmpstring : A255
	VAR CancelType : N01 = 1
	VAR CF_Status : N01
	VAR TABLE_Status : N01
	VAR status :N01
	VAR PrintingStatus : N2
				
	VAR DataFiscalTmp 			:A10
	VAR HorarioTmp 					:A8
	VAR NumeroCuponFiscal 	:A8
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
	
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif
	
	FORMAT DataFiscalTmp AS "20", @year{>02}, @month{>02}, @day{>02}
	FORMAT HorarioTmp AS @hour{>02}, @minute{>02}, @second{>02}
	
	format ValorPagamento as  "{", @tndttl, "}"
	format NumeroCuponFiscal as "{", @cknum{>06} ,"}"
			
	format ValorPagamento as  "{", @ttldue, "}"

  CALL sTEFPayment("0",ValorPagamento, NumeroCuponFiscal, Resultado , CancelType, DataFiscalTmp, HorarioTmp)								
	call sFinishSiTEF(1, NumeroCuponFiscal, DataFiscalTmp ,HorarioTmp)						
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria)
	
endevent

EVENT INQ : 4 // administration TEF
	// work variables
	VAR NumeroCuponFiscal : A8
	VAR Resultado : A6 = "999999"
	VAR Valor : A10
	VAR keypress_confirm : key
	VAR data : A1
	VAR DataFiscalTmp :A10
	VAR HorarioTmp :A08
	VAR tmp1 :A01
	VAR tmp2 :A01
	
	var tmpString :A10000
	var tmpCountP :n9
	
	var tmpConf :A50
	var id :A6	

	FORMAT gbl_LogText AS @line{06}, " ", "Event - INQ 5"
	CALL WriteLogFile(gbl_LogText, cFalse)
	
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif
	
	VAR CancelType : N01 = 1	

	cleararray TEFVoucherBuffer1
	TEFVoucher1LineCnt = 0
	cleararray TEFVoucherBuffer2
	TEFVoucher2LineCnt = 0
	TEFTMedInfo = ""	

	FORMAT Valor AS "{0}"
	FORMAT NumeroCuponFiscal AS "{000000}"	

	CALL sTEFAdminstrative(Valor, NumeroCuponFiscal, Resultado, CancelType, DataFiscalTmp, HorarioTmp)

	IF (TEFVoucher1LineCnt > 0) OR (TEFVoucher2LineCnt > 0)

		format tmpConf as gbl_Conf_IP_Send, ":", gbl_Conf_Porta_Send, ":", 1				
				
		format id as "TEF"		
		
		FORMAT gbl_LogText AS @line{06}, " ", "Envia dados NFCe/SAT: ID: ",id, " IP: ", tmpConf
		CALL WriteLogFile(gbl_LogText, cTrue)	
		
		prompt "Enviando Dados para NFCe Server"	
			
		IF (@wstype = 3)
			dllload hDLLEnvio, "\cf\micros\etc\integrationMicrosCE.dll"
		ELSE
			dllload hDLLEnvio, "integrationMicros.dll"						
		endif	
			
		gbl_envia_dados_retorno = ""
		
		format tmpString as tmpString, gbl_Conf_IP_Print, "|"
			
		for tmpCountP = 1 to TEFVoucher1LineCnt	
			format tmpString as tmpString, TEFVoucherBuffer1[tmpCountP], chr(10)
		endfor
		
		format tmpString as tmpString, " <gui>"
		
		for tmpCountP = 1 to TEFVoucher2LineCnt	
			format tmpString as tmpString, TEFVoucherBuffer2[tmpCountP], chr(10)
		endfor
		
		CALL WriteLogFile(tmpString, cFalse)
			
		// there are SOMETHING to print!		
		//PRINT!
			
		if(hDLLEnvio <> 0)
			dllcall_cdecl hDLLEnvio, CallSat(id, tmpConf, ref tmpString, ref gbl_envia_dados_retorno)					
				
			dllfree hDLLEnvio								
		else 
			errormessage "Erro carregando DLL Envio"
		endif
		//
		CALL sFinishSiTEF(cTrue, NumeroCuponFiscal, "", "")
		
		prompt "Ok..."
		
	ENDIF			
ENDEVENT

Event Inq : 5 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria)
			
	gbl_Check_Processed = 1	
	
endevent

Event Inq: 9
	call InitIFC()
	Errormessage "Interface OK"
endevent

Event Inq : 10 //Cancel SAT
	
	var ChaveCFe[11] 		:A4
	var tmpChave 				:A44
	var tmpMsgCancel 		:A800
	var CPF_CNPJ				:A14
	
	var ClientDocType 	: A1
	
	VAR ListOption : A100
	VAR UserSelection : N50
	VAR PromptTextMessage : A100
	VAR FlagListSelection : A1
	VAR ListLen : N2 = 2
	VAR DOCValidation : N1
	
	VAR localkey : KEY
	
	var retry :N1
	
	var rowCount : N2
	
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif	
	
	FORMAT ListOption as ";1 - Pessoa Fisica;2 - Pessoa Juridica;"
	FORMAT PromptTextMessage as "Selecione o Tipo de Cliente:"
		
	gbl_TamMinimo = 1
	gbl_TamMaximo = 1
	gbl_MenuTitle = PromptTextMessage		
	
	
	IF ClientDocType = ""
		FOREVER
			call sDisplayInput(ListOption, UserSelection)
			IF UserSelection = 0
				Errormessage "E necessario selecionar uma opcao"
			ELSEIF UserSelection = 1
				ClientDocType = "F"
				break
			ELSEIF UserSelection = 2
				ClientDocType = "J"
				break
			ELSEIF UserSelection = -1
				ClientDocType = "B" //(b)ack
				break
			ENDIF
		ENDFOR
	ENDIF	
	
	WINDOW 12, 65, "Digite a Chave do CFe, separando os Campos de 4 Numeros"
			
	DISPLAY 1, 1, "Chave 1 : "
	DISPLAYINPUT 1, 12, ChaveCFe[1],""
	DISPLAY 2, 1, "Chave 2 : "
	DISPLAYINPUT 2, 12, ChaveCFe[2],""
	DISPLAY 3, 1, "Chave 3 : "
	DISPLAYINPUT 3, 12, ChaveCFe[3],""
	DISPLAY 4, 1, "Chave 4 : "
	DISPLAYINPUT 4, 12, ChaveCFe[4],""
	DISPLAY 5, 1, "Chave 5 : "
	DISPLAYINPUT 5, 12, ChaveCFe[5],""
	DISPLAY 6, 1, "Chave 6 : "
	DISPLAYINPUT 6, 12, ChaveCFe[6],""
	DISPLAY 7, 1, "Chave 7 : "
	DISPLAYINPUT 7, 12, ChaveCFe[7],""
	DISPLAY 8, 1, "Chave 8 : "
	DISPLAYINPUT 8, 12, ChaveCFe[8],""
	DISPLAY 9, 1, "Chave 9 : "
	DISPLAYINPUT 9, 12, ChaveCFe[9],""
	DISPLAY 10, 1, "Chave 10 : "
	DISPLAYINPUT 10, 12, ChaveCFe[10],""
	DISPLAY 11, 1, "Chave 11 : "
	DISPLAYINPUT 11, 12, ChaveCFe[11],""			
	DISPLAY 12, 1, "CPF/CNPJ : "
	DISPLAYINPUT 12, 12, CPF_CNPJ,""	
	
	windowedit
	WINDOWCLOSE		
	
	
	format tmpChave as ChaveCFe[1],ChaveCFe[2],ChaveCFe[3],ChaveCFe[4],ChaveCFe[5],ChaveCFe[6],ChaveCFe[7],ChaveCFe[8],ChaveCFe[9],ChaveCFe[10],ChaveCFe[11]
	
	if(len(tmpChave) <> 44)
		call ExitSim(1, "Chave Invalida")
	endif
	
	format tmpMsgCancel as gbl_Conf_IP_Print, "|", tmpChave, "|", ClientDocType ,"|" , CPF_CNPJ
			
	var tmpConf :A50
	var id :A20
	
	FORMAT gbl_LogText AS @line{06}, " ", "INQ 10 - tmpMsgCancel ", tmpMsgCancel
	CALL WriteLogFile(gbl_LogText, cTrue)				
		
	prompt "Processando..."
					
	format tmpConf as gbl_Conf_IP_Send, ":", gbl_Conf_Porta_Send, ":", 0				
	
	format id as "CANCEL"
					
	prompt "Enviando Dados para NFCe Server"	
		
	IF (@wstype = 3)
		dllload hDLLEnvio, "\cf\micros\etc\integrationMicrosCE.dll"
	ELSE
		dllload hDLLEnvio, "integrationMicros.dll"						
	endif	
		
	gbl_envia_dados_retorno = ""
			
	if(hDLLEnvio <> 0)	
	
		dllcall_cdecl hDLLEnvio, CallSat(id, tmpConf, ref tmpMsgCancel, ref gbl_envia_dados_retorno)			
			
		dllfree hDLLEnvio								
	else 
		errormessage "Erro carregando DLL Envio"
	endif
				
	if(gbl_envia_dados_retorno = "1")			
		call ExitSim(1,"DDocumento Informado Nao Cancelado")	
	else
		errormessage "Documento CFe Cancelado com Sucesso"
	endif				
	
endevent

event inq: 11
	
	var tmpConf :A50
	var id :A20
	
	FORMAT gbl_LogText AS @line{06}, " ", "INQ 10 - tmpMsgCancel ", tmpMsgCancel
	CALL WriteLogFile(gbl_LogText, cTrue)				
		
	prompt "Processando..."
					
	format tmpConf as gbl_Conf_IP_Send, ":", gbl_Conf_Porta_Send, ":", 0				
	
	format id as "DESBLOK"
					
	prompt "Enviando Dados para NFCe Server"	
		
	IF (@wstype = 3)
		dllload hDLLEnvio, "\cf\micros\etc\integrationMicrosCE.dll"
	ELSE
		dllload hDLLEnvio, "integrationMicros.dll"						
	endif	
	
	gbl_envia_dados_msg = ""		
	gbl_envia_dados_retorno = ""
			
	if(hDLLEnvio <> 0)	
		
		dllcall_cdecl hDLLEnvio, CallSat(id, tmpConf, ref gbl_envia_dados_msg , ref gbl_envia_dados_retorno)			
			
		dllfree hDLLEnvio								
	else 
		errormessage "Erro carregando DLL Envio"
	endif
			
	if(gbl_envia_dados_retorno = "1")			
		call ExitSim(0,"Erro Processando Desbloqueio")	
	else
		errormessage "Documento CFe Cancelado com Sucesso"
	endif
	
endevent


Event Inq : 51 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria51)
			
	gbl_Check_Processed = 1	
	
endevent
Event Inq : 52 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria52)
			
	gbl_Check_Processed = 1	
	
endevent
Event Inq : 53 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria53)
			
	gbl_Check_Processed = 1	
	
endevent
Event Inq : 54 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria54)
			
	gbl_Check_Processed = 1	
	
endevent
Event Inq : 55 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria55)
			
	gbl_Check_Processed = 1	
	
endevent
Event Inq : 56 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria56)
			
	gbl_Check_Processed = 1	
	
endevent
Event Inq : 57 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria57)
			
	gbl_Check_Processed = 1	
	
endevent
Event Inq : 58 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria58)
			
	gbl_Check_Processed = 1	
	
endevent
Event Inq : 59 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria59)
			
	gbl_Check_Processed = 1	
	
endevent
Event Inq : 60 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria60)
			
	gbl_Check_Processed = 1	
	
endevent
Event Inq : 61 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria61)
			
	gbl_Check_Processed = 1	
	
endevent
Event Inq : 62 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria62)
			
	gbl_Check_Processed = 1	
	
endevent
Event Inq : 63 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria63)
			
	gbl_Check_Processed = 1	
	
endevent
Event Inq : 64 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria64)
			
	gbl_Check_Processed = 1	
	
endevent
Event Inq : 65 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria65)
			
	gbl_Check_Processed = 1	
	
endevent
Event Inq : 66 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria66)
			
	gbl_Check_Processed = 1	
	
endevent
Event Inq : 67 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria67)
			
	gbl_Check_Processed = 1	
	
endevent
Event Inq : 68 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria68)
			
	gbl_Check_Processed = 1	
	
endevent
Event Inq : 69 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria69)
			
	gbl_Check_Processed = 1	
	
endevent

Event Inq : 70 //TMED by INQ - Hotelaria
	
	VAR status :N01
		
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif			
				
	status = 0		
	call EnviaDados_NFCe_SAT(2, status)				//INQ
	if(status = 1) //sucesso
		gbl_Check_Processed = 1
		gbl_try_and_retry = 0		
	else		
		call ExitSim(1,"Erro Processando Documento Eletronico")
	endif
			
	LoadKybdMacro Key(9,gbl_Conf_PagHotelaria70)
			
	gbl_Check_Processed = 1	
	
endevent

event Inq: 6
	
	errormessage "Inq 6"
	
	var tmpCount 				:n9
	var hFile						:n9
	
	var sText						:A256
	
	fopen hFile, "InfoLines.txt", append
	
	fwriteln hFile, "-----"
	
	format sText as "Check ", @chk, " Valor ", @ttldue
	fwriteln hFile, sText
	
	for tmpCount = 1 to @numdtlt		
		format sText as tmpCount, "|", @dtl_id[tmpCount] ,"|" ,@dtl_type[tmpCount], "|",@dtl_name[tmpCount],"|", @dtl_ttl[tmpCount],"|",@Parent_Dtl_Id[tmpCount]
		fwriteln hFile, sText
	endfor
	
	fclose hFile
	
endevent

Event Inq : 90 //Abre Gaveta
	
	var status :n9
	VAR tmpstring : A255
	var tmpConf :A50
	var id :A6	
	
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif
	
	//call LoadDLLEpsonImpressao(status)
		
	//dllcall hDLLEpson, ImprimeTextoTag("<g></g>")
	
	//---
	
	format tmpConf as gbl_Conf_IP_Send, ":", gbl_Conf_Porta_Send, ":", 1				
				
	format id as "TEF"			
	
	prompt "Abrindo Gaveta"	
		
	IF (@wstype = 3)
		dllload hDLLEnvio, "\cf\micros\etc\integrationMicrosCE.dll"
	ELSE
		dllload hDLLEnvio, "integrationMicros.dll"						
	endif	
		
	gbl_envia_dados_retorno = ""
	
	format tmpString as gbl_Conf_IP_Print, "|"
	
	format tmpString as tmpString , "<g></g>"
		
	//for tmpCountP = 1 to TEFVoucher1LineCnt	
	//	format tmpString as tmpString, TEFVoucherBuffer1[tmpCountP], chr(10)
	//endfor
	
	//format tmpString as tmpString, " <gui>"
	
	//for tmpCountP = 1 to TEFVoucher2LineCnt	
	//	format tmpString as tmpString, TEFVoucherBuffer2[tmpCountP], chr(10)
	//endfor
	
	//CALL WriteLogFile(tmpString, cFalse)
		
	// there are SOMETHING to print!		
	//PRINT!
		
	if(hDLLEnvio <> 0)
		dllcall_cdecl hDLLEnvio, CallSat(id, tmpConf, ref tmpString, ref gbl_envia_dados_retorno)					
			
		dllfree hDLLEnvio								
	else 
		errormessage "Erro carregando DLL Envio"
	endif
	
	//---
	
	
	call ExitSIM(0,"")
	
endevent

EVENT INIT
	
	FORMAT gbl_LogText AS @line{06}, " ", "Event - INIT"
	CALL WriteLogFile(gbl_LogText, cTrue)	
	
	gbl_IFC_Status = 0
	
	call InitIFC()
	
	gbl_IFC_Status = 1
	
endevent

EVENT FINAL_TENDER
	//	
	var status :n9
	
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif
		
	prompt "Final Tender"
	
	if (gbl_Check_Processed = 0)
		call EnviaDados_NFCe_SAT(2,status)
	endif
			
	FORMAT gbl_LogText AS @line{06}, " ", "Event - FINAL_TENDER"
	CALL WriteLogFile(gbl_LogText, cTrue)	
	
endevent

EVENT TMED : *
	//
	
	var tmpStr :A10000
	var tmpConf :A50
	var id :A6
	
	VAR status 							:N1
		
	VAR DataFiscalTmp 			:A10
	VAR HorarioTmp 					:A8
	VAR NumeroCuponFiscal 	:A8
	VAR ValorPagamento			:A12	
	VAR Resultado 					:A6 = "999999"
	VAR CancelType 					: N3 = 1	
			
	FORMAT gbl_LogText AS @line{06}, " ", "Event - TMED"
	CALL WriteLogFile(gbl_LogText, cTrue)	
	
	
	gbl_Check_Processed = 0
		
	if (gbl_IFC_Status = 0)
		call InitIFC()
	endif
		
	FORMAT DataFiscalTmp AS "20", @year{>02}, @month{>02}, @day{>02}
	FORMAT HorarioTmp AS @hour{>02}, @minute{>02}, @second{>02}
	
	format ValorPagamento as  "{", @tndttl, "}"
	format NumeroCuponFiscal as "{", @cknum{>06} ,"}"
	
	IF (gbl_Conf_Tef_Enable = 1)
		IF (@tmdnum >= gbl_Conf_TefStart AND @tmdnum <= gbl_Conf_TefEnd)			
			if(@tmdnum >= gbl_Conf_TefStartDEB AND @tmdnum <= gbl_Conf_TefEndDEB)
				CALL sTEFPayment("2",ValorPagamento, NumeroCuponFiscal, Resultado, @tmdnum, CancelType, DataFiscalTmp, HorarioTmp)	
			elseif(@tmdnum >= gbl_Conf_TefStartCRED AND @tmdnum <= gbl_Conf_TefEndCRED)
				CALL sTEFPayment("3",ValorPagamento, NumeroCuponFiscal, Resultado, @tmdnum, CancelType, DataFiscalTmp, HorarioTmp)	
			else
				CALL sTEFPayment("0",ValorPagamento, NumeroCuponFiscal, Resultado, @tmdnum, CancelType, DataFiscalTmp, HorarioTmp)	
			endif
		endif
	endif
		
	if @tndttl >= @ttldue
		
		status = 0		
		call EnviaDados_NFCe_SAT(1, status)				
		if(status = 1) //sucesso
		
			//Gravando InfoLine
			//savechkinfo "NFCE OK"
		
			gbl_Check_Processed = 1
			
			if gbl_TefPayment = 1
				call sFinishSiTEF(1, NumeroCuponFiscal, DataFiscalTmp ,HorarioTmp)
				gbl_TefPayment = 0
			endif
		else
			
			if gbl_TefPayment = 1
				call sFinishSiTEF(0, NumeroCuponFiscal, DataFiscalTmp ,HorarioTmp)
				gbl_TefPayment = 0
				errormessage "Transacao TEF Cancelada. Tentar Novamente"
			endif
			call ExitSim(1,"Erro Processando Documento Eletronico")
		endif
			
		gbl_Check_Processed = 1	
	
	endif	
endevent

EVENT dsc
	
	
	
	
endevent

sub InitIFC()
	var TMPValues1				:A100
	var TMPValues2				:A100
	var h_FileCfg					:N10
	VAR cmd       				:A1024
	VAR cmdparam       		:A1024
	VAR hcmdDLL   				:N12
	VAR retorno   				:N07
	
	
	FORMAT gbl_LogText AS @line{06}, " ", "Event - InitIFC"
	CALL WriteLogFile(gbl_LogText, cTrue)	
		
	@file_separator = "="
		
	IF (@wstype = 3)
		FOPEN h_FileCfg, "\cf\micros\etc\Config_NFCe_SAT.Cfg", read
	ELSE
		FOPEN h_FileCfg, "Config_NFCe_SAT.Cfg", read
	endif
		
	if h_FileCfg = 0		
		call ExitSim(1,"Erro carregando configuracoes")
	endif
	
	WHILE NOT FEOF(h_FileCfg)			
		FORMAT TMPValues1 AS ""
		FORMAT TMPValues2 AS ""
		FREAD h_FileCfg, TMPValues1, TMPValues2
								
		CALL sLoadCFG(TMPValues1, TMPValues2)
	ENDWHILE
	
	FCLOSE h_FileCfg
	
//	 dllload hDLLCmd, "\micros\res\pos\etc\pccmd.dll"
//	 
//	 if(hDLLCmd = 0)
//	 	call ExitSim(1,"Erro carregando DLL de Comandos")
//	 endif
//	 
//	 FORMAT cmd as "SatServerApp.exe"
//	 format cmdparam as ""
//	 //DLLCALL_CDECL hDLLCmd, CMD(ref strCommand, ref retorno) 
//	 DLLCALL_CDECL hDLLCmd, CMD(ref cmd, ref retorno)
	
	gbl_IFC_Status = 1
	
endsub



sub CarregaDLL_NFCE_SAT()
	
	IF (@wstype = 3)
		dllload hDLLEnvio, "\cf\micros\etc\integrationMicrosCE.dll"
	ELSE
		dllload hDLLEnvio, "integrationMicros.dll"						
	endif	
	
endsub

sub EnviaDados_NFCe_SAT(var tipoEvento :N1, ref _status)
	
	var tmpPrintDanfe :N1
	var tmpConf :A50
	var id :A20
	
	var tmpCount 			:N9
	var tmpNFCeOK	:A1
	
	FORMAT gbl_LogText AS @line{06}, " ", "SUB - EnviaDados_NFCe_SAT"
	CALL WriteLogFile(gbl_LogText, cTrue)	
	
	tmpNFCeOK = 0
		
	for tmpCount = 1 to @numdtlt				
		if((@dtl_type[tmpCount] = "I")and(trim(@dtl_name[tmpCount]) = const_NFCE_OK))			
			tmpNFCeOK = 1
			break
		endif
	endfor	
	
	if(tmpNFCeOK = 0)
	
		if(gbl_Conf_ImprimirDanfe = 1)
			tmpPrintDanfe = 0
		else
			tmpPrintDanfe = 1
		endif
		
		prompt "Processando..."
		
		call SetDadosEnvio(tipoEvento)	
		
		format tmpConf as gbl_Conf_IP_Send, ":", gbl_Conf_Porta_Send, ":", tmpPrintDanfe				
		
		//if(gbl_try_and_retry >= 3)
		//	format id as @wsid, @cknum, gbl_try_and_retry
		//else		
			format id as @wsid, @cknum
		//endif
		
		FORMAT gbl_LogText AS @line{06}, " ", "Envia dados NFCe/SAT: ID: ",id, " IP: ", tmpConf
		CALL WriteLogFile(gbl_LogText, cTrue)	
		
		prompt "Enviando Dados para NFCe Server"	
			
		IF (@wstype = 3)
			dllload hDLLEnvio, "\cf\micros\etc\integrationMicrosCE.dll"
		ELSE
			dllload hDLLEnvio, "integrationMicros.dll"						
		endif	
			
		gbl_envia_dados_retorno = ""
				
		if(hDLLEnvio <> 0)
			dllcall_cdecl hDLLEnvio, CallSat(id, tmpConf, ref gbl_envia_dados_msg, ref gbl_envia_dados_retorno)			
				
			dllfree hDLLEnvio								
		else 
			errormessage "Erro carregando DLL Envio"
		endif
			
		FORMAT gbl_LogText AS @line{06}, " ", "Retorno Dados ",gbl_envia_dados_msg
		CALL WriteLogFile(gbl_LogText, cTrue)	
			
		FORMAT gbl_LogText AS @line{06}, " ", "Retorno QR ",gbl_envia_dados_QrCode
		CALL WriteLogFile(gbl_LogText, cTrue)	
			
		FORMAT gbl_LogText AS @line{06}, " ", "Retorno BAR Code ",gbl_envia_dados_BaCode
		CALL WriteLogFile(gbl_LogText, cTrue)	
			
		FORMAT gbl_LogText AS @line{06}, " ", "Retorno Retorno ",gbl_envia_dados_retorno
		CALL WriteLogFile(gbl_LogText, cTrue)	
						
		errormessage mid(gbl_envia_dados_msg,1,38)	
			
		errormessage "ret: " ,gbl_envia_dados_retorno	
			
		if(gbl_envia_dados_retorno = "1")			
			_status = 0
		else
			_status = 1 //Passou				
			
			call SaveCheckInfo(const_NFCE_OK)		
			
		endif	
	else
		Errormessage "NFCe ja autorizado para essa Conta"
		_status = 1 //Passou		
	endif
		
endsub

sub SetDadosEnvio(var tipoEvento :n1)
	
	var tmpCount 	:n9 = 1
	var tmpCountH :n9 = 1
	var tmpCountI :n9 = 1
	var tmpCountP :n9 = 1
	
	var tmpTEFEnvia :A2000
	
	FORMAT gbl_LogText AS @line{06}, " ", "SUB - SetDadosEnvio"
	CALL WriteLogFile(gbl_LogText, cTrue)	
	
	format gbl_envia_dados_msg as ""	
	gbl_envio_header_count = 0
	gbl_envio_itens_count = 0
	gbl_envio_payments_count = 0	
	
	call SetDadosEnvioItens()	
	//call SetDadosEnvioPayments()
	call SetDadosEnvioHeader()
	
	if(tipoEvento = 1)	
		call SetDadosEnvioPayments(1,@tmdnum, "Di", @tndttl)
	elseif(tipoEvento = 2)	
		call SetDadosEnvioPayments(1,"99", "Outro", @ttlDue)
	endif
	
	for tmpCountH = 1 to gbl_envio_header_count
		tmpCount = tmpCount + 1
		format gbl_envia_dados_msg as gbl_envia_dados_msg,  gbl_envio_header[tmpCountH], chr(10)
		CALL WriteLogFile(gbl_envio_header[tmpCountH], cFalse)		
	endfor
	
	for tmpCountI = 1 to gbl_envio_itens_count
		tmpCount = tmpCount + 1
		format gbl_envia_dados_msg as gbl_envia_dados_msg, gbl_envio_itens[tmpCountI], chr(10)
		CALL WriteLogFile(gbl_envio_itens[tmpCountI], cFalse)		
	endfor
		
	for tmpCountP = 1 to gbl_envio_payments_count
		tmpCount = tmpCount + 1
		format gbl_envia_dados_msg as gbl_envia_dados_msg, gbl_envio_payments[tmpCountp], chr(10)
		CALL WriteLogFile(gbl_envio_payments[tmpCountP], cFalse)		
	endfor
	
	for tmpCountP = 1 to TEFVoucher1LineCnt	
		format tmpTEFEnvia as tmpTEFEnvia, TEFVoucherBuffer1[tmpCountP], chr(10)
	endfor
	format tmpTEFEnvia as tmpTEFEnvia, "|"
	for tmpCountP = 1 to TEFVoucher2LineCnt	
		format tmpTEFEnvia as tmpTEFEnvia, TEFVoucherBuffer2[tmpCountP], chr(10)
	endfor
	
	format gbl_envia_dados_msg as gbl_envia_dados_msg, "?T1|" , tmpTEFEnvia
	
	CALL WriteLogFile("Dados de Envio Completo ---> ", cFalse)
	CALL WriteLogFile(gbl_envia_dados_msg, cFalse)
	
endsub

sub SetDadosEnvioHeader()
	FORMAT gbl_LogText AS @line{06}, " ", "SUB - SetDadosEnvioHeader"
	CALL WriteLogFile(gbl_LogText, cTrue)	
		
	call SetDadosEnvioHeader_Cliente()
	call SetDadosEnvioHeader_Check()	
	
endsub

sub SetDadosEnvioHeader_Check()
	
	FORMAT gbl_LogText AS @line{06}, " ", "SUB - SetDadosEnvioHeader_Check"
	CALL WriteLogFile(gbl_LogText, cTrue)	
	
	var checkNum 						:N9
	var checkMesa						:N9
	var checkEmp						:A100	
	var checkWS_ID					:N4
	var tmpPrintDanfe				:N1
	
	VAR Hotel_Client_Info		:A50
	
	VAR checkCharge					:$12 = 0
	
	VAR tmpString						:A500
	
	var tmpAliqString				:A48
	
	if(gbl_Conf_ImprimirDanfe = 1)
		tmpPrintDanfe = 0
	else
		tmpPrintDanfe = 1
	endif
	
	//Lendo dados de hotelaria e Acresc
		for tmpCount = 1 to @numdtlt		
			if(@dtl_type[tmpCount] = "R")or (mid(@dtl_name[tmpCount],1,1)="~")				
				format Hotel_Client_Info as "Apto/Hosp:" ,mid(trim(@dtl_name[tmpCount]),2,100), "\"
				format gbl_LogText as "Type: ",@dtl_type[tmpCount] , " Name: ", @dtl_name[tmpCount]
				call WriteLogFile(gbl_LogText, cfalse);						
			
			elseif(@dtl_type[tmpCount] = "S")
				checkCharge = checkCharge + @dtl_ttl[tmpCount]
			endif
		endfor
	//
		
	checkCharge = checkCharge + @autosvc
		
	checkNum = @cknum
	checkMesa = @tblid
	checkEmp = @ckemp_chkname
	checkWS_ID = @wsid
	
	format tmpString as "WS: ", checkWS_ID, " / Conta: ", checkNum, " / Mesa: ", checkMesa, " / Emp.: ",checkEmp 
	
	call sNewAliqFunction(tmpAliqString)	
	
	format tmpString as tmpString, " \ ", tmpAliqString
	
	if(Len(gbl_Linha1) > 0)
		format tmpString as tmpString, " \ ", gbl_Linha1
	endif
	if(Len(gbl_Linha2) > 0)
		format tmpString as tmpString, " \ ", gbl_Linha2
	endif
	if(Len(gbl_Linha3) > 0)
		format tmpString as tmpString, " \ ", gbl_Linha3
	endif
	if(Len(gbl_Linha4) > 0)
		format tmpString as tmpString, " \ ", gbl_Linha4
	endif
	if(Len(gbl_Linha5) > 0)
		format tmpString as tmpString, " \ ", gbl_Linha5
	endif
	if(Len(gbl_Linha6) > 0)	
		format tmpString as tmpString, " \ ", gbl_Linha6
	endif
	if(Len(gbl_Linha7) > 0)
		format tmpString as tmpString, " \ ", gbl_Linha7
	endif
	if(Len(gbl_Linha8) > 0)
		format tmpString as tmpString, " \ ", gbl_Linha8
	endif
	if(Len(gbl_Linha9) > 0)
		format tmpString as tmpString, " \ ", gbl_Linha9	
	endif
	
	var tmpS :A100
	
	format tmpS as mid(Hotel_Client_Info,2,100)
	
	
	gbl_envio_header_count = gbl_envio_header_count + 1
	format gbl_envio_header[gbl_envio_header_count] as "H", gbl_envio_header_count, "|", tmpPrintDanfe , "|", checkWS_ID, "|",checkNum, "|", checkMesa, "|", checkEmp, "|", checkCharge, "|", gbl_Conf_IP_Print
	
	
	gbl_envio_header_count = gbl_envio_header_count + 1
	format gbl_envio_header[gbl_envio_header_count] as "H", gbl_envio_header_count, "|", Hotel_Client_Info, "|",tmpString
	
	
	
endsub

sub SetDadosEnvioHeader_Cliente()
		
	VAR keypress_confirm : key
	
	VAR ClientDocType 			: A1 // F = Fisica, J = Juridica, O = Other System like Opera
	
	VAR ClientNome :A35
	
	var ClienteDoc :A35
	var ClientIE		:A35
	
	VAR ClientEndereco :A35
	VAR ClientNumero :A5
	VAR ClientComeplemento :A35
	VAR ClientBairro :A35
	VAR ClientCEP :A9
	VAR ClientCidade :A35
	VAR ClientEstado :A2
	VAR ClientEmail :A40
	
	VAR tmpCount											:N9	
	VAR Hotel_Client_Info							:A200	
	VAR Hotel_CLient_InfoArray[2]			:A100
			
	FORMAT gbl_LogText AS @line{06}, " ", "SUB - SetDadosEnvioHeader_Cliente"
	CALL WriteLogFile(gbl_LogText, cTrue)	
	
	IF gbl_config_CapturaDadosCliente = cTrue
		call sYesOrNo(keypress_confirm, "Capturar Dados do Cliente?")
		
		FORMAT ClienteDoc as ""
		FORMAT ClientDocType as ""
		FORMAT ClientNome as ""
		FORMAT ClientEndereco as ""
		FORMAT ClientNumero as ""
		FORMAT ClientComeplemento as ""
		FORMAT ClientBairro as ""
		FORMAT ClientCEP as ""
		FORMAT ClientCidade as ""
		FORMAT ClientEstado as ""
		FORMAT ClientEmail as ""		
				
		
		if GBL_CONF_DADOSPAGANTE = 1
			//Lendo dados de hotelaria
			for tmpCount = 1 to @numdtlt
				if(@dtl_type[tmpCount] = "R")or (mid(@dtl_name[tmpCount],1,1)="~")				
					format Hotel_Client_Info as mid(trim(@dtl_name[tmpCount]),2,100)
					
					split Hotel_Client_Info, "/", Hotel_Client_InfoArray[1],Hotel_Client_InfoArray[2]
					
					format gbl_LogText as "Type: ",@dtl_type[tmpCount] , " Name: ", @dtl_name[tmpCount]
					call WriteLogFile(gbl_LogText, cfalse);						
				
				//elseif(@dtl_type[tmpCount] = "S")
				//	checkCharge = checkCharge + @dtl_ttl[tmpCount]
				endif
			endfor
			//
			//Call sGetClientDataFromFile(ClientDocType, ClienteDoc, ClientIE, ClientNome, ClientEndereco, ClientNumero, ClientComeplemento, ClientBairro, ClientCEP, ClientCidade, ClientEstado, ClientEmail)
			
			var hFileDadosCliente :N9
			var tmpStringDadosClienteLine :A256
			var tmpStringDadosClienteList[14] :A100
			
			FOPEN hFileDadosCliente, "DadosPaganteOpera.txt", read
			
			WHILE NOT FEOF(hFileDadosCliente)
				freadln hFileDadosCliente, tmpStringDadosClienteLine
				
				split tmpStringDadosClienteLine, "|", tmpStringDadosClienteList[1],tmpStringDadosClienteList[2],tmpStringDadosClienteList[3],tmpStringDadosClienteList[4],tmpStringDadosClienteList[5],tmpStringDadosClienteList[6],tmpStringDadosClienteList[7],tmpStringDadosClienteList[8],tmpStringDadosClienteList[9],tmpStringDadosClienteList[10],tmpStringDadosClienteList[11],tmpStringDadosClienteList[12],tmpStringDadosClienteList[13],tmpStringDadosClienteList[14]
				
				//errormessage tmpStringDadosClienteList[1]," | ", Hotel_Client_InfoArray[1]
				
				if tmpStringDadosClienteList[1] = Hotel_Client_InfoArray[1] //Numero do Apartamento
					//errormessage "achou"
					
					FORMAT ClientDocType as "J"
					FORMAT ClienteDoc as tmpStringDadosClienteList[2]
					FORMAT ClientNome as tmpStringDadosClienteList[3]
					FORMAT ClientEndereco as tmpStringDadosClienteList[4]
					FORMAT ClientNumero as tmpStringDadosClienteList[5]
					FORMAT ClientComeplemento as tmpStringDadosClienteList[6]
					FORMAT ClientBairro as tmpStringDadosClienteList[7]
					
					FORMAT ClientCidade as tmpStringDadosClienteList[8]
					FORMAT ClientEstado as tmpStringDadosClienteList[10]
					FORMAT ClientCEP as tmpStringDadosClienteList[11]
					FORMAT ClientEmail as tmpStringDadosClienteList[14]	
					break				
				endif
				
			endwhile			
		endif	
		
		IF keypress_confirm = @key_enter
			Call sGetClientData(ClientDocType, ClienteDoc, ClientIE, ClientNome, ClientEndereco, ClientNumero, ClientComeplemento, ClientBairro, ClientCEP, ClientCidade, ClientEstado, ClientEmail)
		ENDIF
			
		gbl_envio_header_count = gbl_envio_header_count + 1
		
		FORMAT gbl_envio_header[gbl_envio_header_count] as "H", gbl_envio_header_count, "|", ClientDocType, "|", ClientNome, "|", ClienteDoc, "|",ClientEndereco, "|", ClientNumero, "|", ClientComeplemento, "|", ClientBairro, "|", ClientCEP, "|", ClientCidade, "|", ClientEstado, "|",ClientEmail, "|",ClientIE		
	
endsub

sub SetDadosEnvioItens()
	var tmpCount 					:N9
	var tmpCountDesc 			:N9
	//var contItens			:N9 = 0
	
	var itemCode		  		:N9
	var itemDesc 					:A100
	var itemQty 					:N9
	var itemAliq 					:A10
	var itemValorUni			:$12
	var itemValorTotal 		:$12
	var itemDesconto			:$12
	
	var itemNCM						:A10
	
	var tmpParentID 			:N9
	
	FORMAT gbl_LogText AS @line{06}, " ", "SUB - SetDadosEnvioItens"
	CALL WriteLogFile(gbl_LogText, cTrue)
	
	for tmpCount = 1 to @numdtlt		
		IF (@dtl_type[tmpCount]="M") AND (@dtl_ttl[tmpCount] > 0) AND (@dtl_is_void[tmpCount] = 0) //Verifica Menu Item
			
			gbl_envio_itens_count = gbl_envio_itens_count + 1 //Incrementa Itens Encontrados
			
			//Verifica por Descontos Associdados a esse item.			
						
			//format gbl_LogText as "Procurando Descontos Item: ", tmpCount, " | ", @dtl_name[tmpCount]
			//	call WriteLogFile(gbl_LogText, cfalse);				
			
			//Olhando Descontos
			
			if(@dsc <> 0)				
			
				itemDesconto = 0
				
				for tmpCountDesc = 1 to @numdtlt
						
					//	format gbl_LogText as "Loop Desconto: ", tmpCountDesc
					//	call WriteLogFile(gbl_LogText, cfalse);		
													
					IF (@dtl_type[tmpCountDesc] = "D") and (@Parent_Dtl_Id[tmpCountDesc] = @dtl_id[tmpCount]) //Verifica Menu Item
											
						itemDesconto = itemDesconto + @dtl_ttl[tmpCountDesc]
						
						tmpParentID = @Parent_Dtl_Id[tmpCountDesc]
						
						format gbl_LogText as "Desconto: ", tmpCountDesc, " Valor: ",  @dtl_ttl[tmpCountDesc], " Seq: " ,@dtl_sequence[tmpCountDesc], " ParentID: ", @Parent_Dtl_Id[tmpCountDesc]
						call WriteLogFile(gbl_LogText, cfalse);
						
						format gbl_LogText as tmpParentID, " Item: ",@dtl_name[tmpParentID] , " Valor: ",  @dtl_ttl[tmpParentID], " Seq: " ,@dtl_sequence[tmpParentID]
						call WriteLogFile(gbl_LogText, cfalse);
					endif
				endfor
			endif
			//
			
			itemCode = @dtl_object[tmpCount]
			itemDesc = @dtl_name[tmpCount]
			itemQty = @dtl_qty[tmpCount]
						
			format itemNCM as mid(@dtl_name_two[tmpCount],1,8)
			
			if(itemQty < 1)
				itemQty = 1
			endif
			
			itemValorTotal = @dtl_ttl[tmpCount]
			itemValorUni = itemValorTotal / itemQty
			
			itemDesconto = itemDesconto * -1
			
			CALL GetItemTaxRate(itemAliq, @dtl_taxtype[tmpCount])			
			
			//Inclui Item no Array de Envio
			format gbl_envio_itens[gbl_envio_itens_count] as "I|" , itemCode, "|", itemDesc, "|", itemQty, "|", itemValorUni, "|", itemValorTotal, "|", itemAliq, "|", itemDesconto, "|", itemNCM
			
		endif			
	endfor	
endsub

sub SetDadosEnvioPayments(var tipoEvento: n1, var payCode :N9, var payDesc :A50, var payValorTotal :$12)
		
	//TipoEvento: 1 = TMED | 2 = FinalTender 
		
	var tmpCount 			:N9	
	var tmpPayCode    :N9
	var tmpPayDesc		:A50
	var tmpPayValorTotal :$12
	
	FORMAT gbl_LogText AS @line{06}, " ", "SUB - SetDadosEnvioPayments - Tipo ", tipoEvento
	CALL WriteLogFile(gbl_LogText, cTrue)
	
	for tmpCount = 1 to @numdtlt		
		IF (@dtl_type[tmpCount]="T") AND (@dtl_ttl[tmpCount] > 0) AND (@dtl_qty[tmpCount] > 0) AND (@dtl_is_void[tmpCount] = 0) //Verifica Menu Item
			
			gbl_envio_payments_count = gbl_envio_payments_count + 1 //Incrementa Itens Encontrados
			
			tmpPayCode = @dtl_object[tmpCount]
			tmpPayDesc = @dtl_name[tmpCount]			
			tmpPayValorTotal = @dtl_ttl[tmpCount]						
			
			//Inclui Item no Array de Envio
			format gbl_envio_payments[gbl_envio_payments_count] as "P|" , tmpPayCode, "|", tmpPayDesc, "|", tmpPayValorTotal
			
		endif			
	endfor	
	
	if(tipoEvento = 1) //retirando TESTES SAT
		gbl_envio_payments_count = gbl_envio_payments_count + 1 //Incrementa Itens Encontrados
		format gbl_envio_payments[gbl_envio_payments_count] as "P|" , payCode, "|", payDesc, "|", payValorTotal	
	endif
	
endsub

SUB ExitSIM(var tipoErro :n2, var msgErro :A50)
	
	FORMAT gbl_LogText AS @line{06}, " ", "SUB - ExitSIM - TipoErro: ", TipoErro, " MSG: ", msgErro 
	CALL WriteLogFile(gbl_LogText, cTrue)
	//tipoErro
	// 0 = ExitContinue
	// 1 = ExitCancel
	
	prompt "Pronto..."
	
	dllfree hDLLEpson
	dllfree hDLLEnvio
	dllfree hDLLSitef	
	
	if(tipoErro = 0)
		ExitContinue
	elseif (tipoErro = 1)
		errormessage msgErro
		ExitCancel
	endif
	
endsub

SUB ImprimirEpsonNF(VAR txt : A100, ref _status)
	
	FORMAT gbl_LogText AS @line{06}, " ", "SUB - ImprimirEpsonNF - MSG: " ,txt
	CALL WriteLogFile(gbl_LogText, cTrue)
	
	call LoadDLLEpsonImpressao(_status)
	if _status = 0		
		call ExitSim(1,"Erro Carregando DLL Epson") //TODO
	else
		dllcall hDLLEpson, ImprimeTexto(txt)
		FORMAT gbl_LogText AS @line{06}, " ", "Epson - ImprimeTexto " ,txt
		CALL WriteLogFile(gbl_LogText, cTrue)
	endif
	
endsub

//Carregar DLL Epson Impressao
SUB LoadDLLEpsonImpressao(ref _status)
	
	FORMAT gbl_LogText AS @line{06}, " ", "SUB - LoadDLLEpsonImpressao"
	CALL WriteLogFile(gbl_LogText, cTrue)
	
	
	
	if hDLLEpson = 0
		dllload hDLLEpson, gbl_dllEpsonImpressao
		
		//dllcall hDLLEpson, FechaPorta("USB")
				
		dllcall hDLLEpson, IniciaPorta(gbl_Conf_IP_Send)		
		
	endif
	
	if hDLLEpson = 0
		_status = 0 //retornando ERRO
	endif	
	
endsub

//***********************************************************************************
// Get the percentage of Tax
// _DTLTAXTYPE recive the Hex of Tax Item
//***********************************************************************************
SUB GetItemTaxRate(REF _ItemTaxRatePercentage, VAR _DTLTAXTYPE : A2)
	
	FORMAT gbl_LogText AS @line{06}, " ", "SUB - GetItemTaxRate", _DTLTAXTYPE
	CALL WriteLogFile(gbl_LogText, cTrue)
	
	VAR TaxRatePosition : N1	

	IF _DTLTAXTYPE = "80"
		TaxRatePosition = 1
	ELSEIF _DTLTAXTYPE = "40"
		TaxRatePosition = 2
	ELSEIF _DTLTAXTYPE = "20"
		TaxRatePosition = 3
	ELSEIF _DTLTAXTYPE = "10"
		TaxRatePosition = 4
	ELSEIF _DTLTAXTYPE = "08"
		TaxRatePosition = 5
	ELSEIF _DTLTAXTYPE = "04"
		TaxRatePosition = 6
	ELSEIF _DTLTAXTYPE = "02"
		TaxRatePosition = 7
	ELSEIF _DTLTAXTYPE = "01"
		TaxRatePosition = 8
	ELSE
		TaxRatePosition = 0
	ENDIF
	IF TaxRatePosition = 0
		FORMAT _ItemTaxRatePercentage AS "NULL"
	ELSEIF TaxRatePosition = 7 // Set to FF SUBstitution Rate
		FORMAT _ItemTaxRatePercentage AS "FF"
	ELSEIF TaxRatePosition = 8 // Set to NN NON ICMS Rate
		FORMAT _ItemTaxRatePercentage AS "NN"
	ELSE
		FORMAT _ItemTaxRatePercentage AS @taxrate[TaxRatePosition]
	ENDIF

ENDSUB

//***********************************************************************************
// Write Interface Log File
//***********************************************************************************
SUB WriteLogFile(VAR _MensageErro : A20000 , VAR Force_ : N1)
	
	
	
	VAR hLogFile : N12 = 0
	VAR LogFileName : A50
	VAR StringToWrite : A20000

	@File_Bfrsize = 8192

	IF gbl_config_LOG_INTERFACE = cTRUE or Force_ = cTRUE		
		
		FORMAT LogFileName AS "PMS", @pmsnumber{>03}, ".log.txt"		

		if len(_MensageErro) > 3900
			FORMAT StringToWrite AS @day{02}, "/",@month{02}, "/20",@year{02}, " ", @hour{02}, ":", @minute{02}, ":", @second{>02}, " [", @Varused{<09}, "/",@Varspace{<09},"]- Chk: ", @cknum{06}, " Emp: ", @ckemp{06}, " - ", Mid(_MensageErro,1,3900)
		else
			FORMAT StringToWrite AS @day{02}, "/",@month{02}, "/20",@year{02}, " ", @hour{02}, ":", @minute{02}, ":", @second{>02}, " [", @Varused{<09}, "/",@Varspace{<09},"]- Chk: ", @cknum{06}, " Emp: ", @ckemp{06}, " - ", _MensageErro
		endif

		FOPEN hLogFile, LogFileName, Append

		IF hLogFile <> 0
			FWRITELN hLogFile, StringToWrite
			FCLOSE hLogFile
		ELSE
			errormessage "Nao foi possivel abrir o arquivo de LOG"
		ENDIF
	ENDIF
	@File_Bfrsize = 4096
ENDSUB

SUB sGetClientData(ref ClientDocType, ref ClientDoc, ref ClientIE , ref ClientNome, ref ClientEndereco, ref ClientNumero, ref ClientComeplemento, ref ClientBairro, ref ClientCEP, ref ClientCidade, ref ClientEstado, ref ClientEmail)
	
	VAR ListOption : A100
	VAR UserSelection : N50
	VAR PromptTextMessage : A100
	VAR FlagListSelection : A1
	VAR ListLen : N2 = 2
	VAR DOCValidation : N1
	
	VAR localkey : KEY
	
	var retry :N1
	

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sGetClientData"
	CALL WriteLogFile(gbl_LogText, cFalse)

	FORMAT ListOption as ";1 - Pessoa Fisica;2 - Pessoa Juridica;"
	FORMAT PromptTextMessage as "Selecione o Tipo de Cliente:"
		
	gbl_TamMinimo = 1
	gbl_TamMaximo = 1
	gbl_MenuTitle = PromptTextMessage		
		
	
	FOREVER
		IF ClientDocType = ""
			FOREVER
				call sDisplayInput(ListOption, UserSelection)
				IF UserSelection = 0
					Errormessage "E necessario selecionar uma opcao"
				ELSEIF UserSelection = 1
					ClientDocType = "F"
					break
				ELSEIF UserSelection = 2
					ClientDocType = "J"
					break
				ELSEIF UserSelection = -1
					ClientDocType = "B" //(b)ack
					break
				ENDIF
			ENDFOR
		ENDIF		
		
						
		IF ClientDocType = "F"
			WINDOW 12, 65, "Dados do Cliente:"
			
			DISPLAY 1, 1, "CPF "
			DISPLAYINPUT 1, 12, ClientDoc{:###.###.### ##}, ": "
			
			if(gbl_Conf_SAT_NFCE = gbl_const_NFCE) //dados apenas NFCE
			
				DISPLAY 2, 1, "Nome"
				DISPLAYINPUT 2, 12, ClientNome, ": "
				
				DISPLAY 3, 1, "Email "
				DISPLAYINPUT 3, 12, ClientEmail, ": "		
				
				DISPLAY 4, 1, "Endereco "
				DISPLAYINPUT 4, 12, ClientEndereco, ": "
				
				DISPLAY 5, 1, "Numero "
				DISPLAYINPUT 5, 12, ClientNumero, ": "
				
				DISPLAY 6, 1, "Comp "
				DISPLAYINPUT 6, 12, ClientComeplemento, ": "
				
				DISPLAY 7, 1, "Bairro "
				DISPLAYINPUT 7, 12, ClientBairro, ": "
				
				DISPLAY 8, 1, "CEP "
				DISPLAYINPUT 8, 12, ClientCEP, ": "
				
				DISPLAY 9, 1, "Cidade "
				DISPLAYINPUT 9, 12, ClientCidade, ": "
				
				DISPLAY 10, 1, "Estado "
				DISPLAYINPUT 10, 12, ClientEstado, ": "					
				
			endif
			
		ELSEIF ClientDocType = "J"
			WINDOW 12, 65, "Dados da Empresa:"
			
			DISPLAY 1, 1, "CNPJ "
			DISPLAYINPUT 1, 15, ClientDoc{:##.###.###/#### ##}, ": "
			
			if(gbl_Conf_SAT_NFCE = gbl_const_NFCE) //dados apenas NFCE
			
				//DISPLAY 2, 1, "I.E. "
				//DISPLAYINPUT 2, 15, ClientIE{:##.###.###/#### ##}, ": "
				
				DISPLAY 2, 1, "Razao Social"
				DISPLAYINPUT 2, 15, ClientNome, ": "
				
				DISPLAY 3, 1, "Email "
				DISPLAYINPUT 3, 15, ClientEmail, ": "		
				
				DISPLAY 4, 1, "Endereco "
				DISPLAYINPUT 4, 15, ClientEndereco, ": "
				
				DISPLAY 5, 1, "Numero "
				DISPLAYINPUT 5, 15, ClientNumero, ": "
				
				DISPLAY 6, 1, "Complemento "
				DISPLAYINPUT 6, 15, ClientComeplemento, ": "
				
				DISPLAY 7, 1, "Bairro "
				DISPLAYINPUT 7, 15, ClientBairro, ": "
				
				DISPLAY 8, 1, "CEP "
				DISPLAYINPUT 8, 15, ClientCEP, ": "
				
				DISPLAY 9, 1, "Cidade "
				DISPLAYINPUT 9, 15, ClientCidade, ": "
				
				DISPLAY 10, 1, "Estado "
				DISPLAYINPUT 10, 15, ClientEstado, ": "		
			endif	
			
		ENDIF
		
		IF ClientDocType = "B"
			break
		ENDIF
				
		windowedit
		WINDOWCLOSE		
				
		call sValida_CNPJCPF(ClientDocType, ClientDoc, DOCValidation)
		
			if(gbl_Conf_SAT_NFCE = gbl_const_NFCE) //dados apenas NFCE
	
			IF (DOCValidation >= 0 and len(ClientNome) > 2)
				Break
			ELSEIF len(ClientNome) <= 2
				INFOMESSAGE "Nome Digitado Invalido"			
			ELSEIF ClientDocType = "J"
				INFOMESSAGE "CNPJ digitado invalido"
			ELSEIF ClientDocType = "F"
				INFOMESSAGE "CPF digitado invalido"		
			ENDIF
			
			retry = retry + 1
			
			if(retry = 3)
				INFOMESSAGE "Enviando sem dados do Cliente"		
				ClientDoc = ""
				ClientNome = ""
				break
			endif
			
			IF LEN(trim(ClientDoc)) = 0 and LEN(trim(ClientNome)) < 2
				ClientDocType = ""
			ENDIF
		else
			break
						
	ENDFOR	
				
ENDSUB

SUB sValida_CNPJCPF(ref _ClientDocType, ref doc_valor, ref resultado)
	//resultado 0 ok, resultado = 1 erro tamanho, resultado = 2 erro validacao dados
	VAR posicao_inicial : N2
	VAR cont_array : N2
	VAR soma : N9
	VAR fator : N2
	VAR resultado_parcial : N9
	VAR valor_posicao : N2
	VAR valor_posicao_string : A1
	VAR resultado_1 : N9
	VAR resultado_2 : N9
	VAR resultado_3 : N9
	VAR DigitoInicial : N9

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sValida_CNPJCPF(", _ClientDocType, ", ", doc_valor, ", ", resultado, ")"
	CALL WriteLogFile(gbl_LogText, cFalse)

	resultado = 0
	IF (len(doc_valor) <> 14) and (_ClientDocType = "J")
		errormessage "Erro no tamanho do CNPJ, tamanho certo e de 14 casas"
		resultado = -1
		return
	ELSEIF (len(doc_valor) <> 11) and (_ClientDocType = "F")
		errormessage "Erro no tamanho do CPF, tamanho certo e de 11 casas"
		resultado = -1
		return
	ENDIF
	IF _ClientDocType = "J"
		DigitoInicial = 12
	ELSE
		DigitoInicial = 9
	ENDIF

	FOR posicao_inicial = DigitoInicial to (DigitoInicial + 1)
		fator = 9
		soma = 0
		FOR cont_array = posicao_inicial to 1 step -1
			resultado_parcial = mid(doc_valor,cont_array, 1) * fator
			soma = soma + resultado_parcial
			fator = fator - 1
			IF (fator < 2) and (_ClientDocType = "J")
				fator = 9
			ELSEIF (fator < 0) and (_ClientDocType = "F")
				fator = 9
			ENDIF
		ENDFOR
		//calcula o resto da operacao soma / 11
		valor_posicao = soma % 11
		IF valor_posicao > 9
			valor_posicao = 0
		ENDIF
		//compara valores
		IF valor_posicao <> mid(doc_valor,posicao_inicial+1,1)
			resultado = -2
			return
		ENDIF
	ENDFOR
ENDSUB

SUB sYesOrNo(REF keypress_confirm, VAR string :A255)
	VAR data : A255
	VAR ask : A255

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sYesOrNo(", keypress_confirm, ", ", string , ")"
	CALL WriteLogFile(gbl_LogText, cFalse)

	FORMAT ask AS TRIM(string), "? (SIM/NAO)"
	FOREVER
		PROMPT TRIM(string)
		WINDOW 4, gbl_WndColSize[@wstype], "Responda"//TRIM(string)
		DISPLAY 1, 2, ask
		INPUTKEY keypress_confirm, data, ask
		IF (keypress_confirm <> @key_enter) and (keypress_confirm <> @key_clear)
			errormessage "E necessario digitar em SIM ou Nao!!!"
		ELSE
			break
		ENDIF
	ENDFOR
	WINDOWCLOSE	
ENDSUB

SUB sDisplayInput(REF _buffer, ref InputAnswer)
	VAR maxRow : N5 = 12
	VAR maxCol : N5 = 65//70
	VAR bufferLength : N5
	VAR Row : N5 = 1
	VAR Column : N5 = 2
	VAR SeparatorPosition : N5
	VAR localkey : KEY
	VAR titlearray[10] : A255
	VAR i : N5
	VAR tmpline : A255
	VAR tmpcolcount : N5 = 1
	VAR keypress_confirm : KEY
	
	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sDisplayInput(", _buffer, ", ", InputAnswer, ")"
	CALL WriteLogFile(gbl_LogText, cFalse)

	IF instr(1, gbl_MenuTitle, "\") > 0
		split gbl_MenuTitle, "\", titlearray[1], titlearray[2], titlearray[3], titlearray[4], titlearray[5], \
		titlearray[6], titlearray[7], titlearray[8], titlearray[9], titlearray[10]
		MenuTitle = " "
	ENDIF

	FOREVER
		WINDOW maxRow, maxCol, gbl_MenuTitle
		IF len(trim(titlearray[1])) > 0
			FOR i = 1 to 10
				IF trim(titlearray[i]) <> ""
					DISPLAY Row, 1, trim(titlearray[i]){=63}//{=68}
					Row = Row + 1
				ELSE
					break
				ENDIF
			ENDFOR
			DISPLAY Row, 1, " "
			Row = Row + 1
		ENDIF
		WHILE Row < maxRow
			SeparatorPosition = INSTR(Column, _buffer, ";")
			IF SeparatorPosition = 0
				BREAK
			ENDIF
			SeparatorPosition = SeparatorPosition - Column
			tmpline = MID(_buffer, Column, SeparatorPosition)

			IF instr(1, tmpline, chr(10)) > 0
				FOREVER
					IF instr(tmpcolcount, tmpline, chr(10)) = 0
						DISPLAY Row, 1, mid(tmpline, tmpcolcount, len(tmpline) - tmpcolcount)
					ELSE
						DISPLAY Row, 1, mid(tmpline, tmpcolcount, instr(tmpcolcount, tmpline, chr(10) - tmpcolcount)
					ENDIF
					tmpcolcount = instr(tmpcolcount, tmpline, chr(10)) + 1
					Row = Row + 1

					IF tmpcolcount = 1
						break
					ENDIF
				ENDFOR
			ELSE
				DISPLAY Row, 1, tmpline
			ENDIF

			Column = Column + SeparatorPosition + 1
			Row = Row + 1
		ENDWHILE
		INPUTKEY localkey, InputAnswer, gbl_MenuTitle
		WINDOWCLOSE
		IF localkey = @key_enter
			IF (LEN(InputAnswer) >= gbl_TamMinimo) and (LEN(InputAnswer) <= gbl_TamMaximo)
				BREAK
			ELSE
				Row = 1
				Column = 2
				tmpcolcount = 1
			ENDIF
		ELSEIF localkey = @key_clear
			call sYesOrNo(keypress_confirm, "Deseja cancelar?")
			IF keypress_confirm = @key_enter
				InputAnswer = -1
				break
			ENDIF
		ELSE
			// reset variable
			Row = 1
			Column = 2
			tmpcolcount = 1
		ENDIF
	ENDFOR
ENDSUB


// ----------------------- TEF ROUTINES ---------------------------------------
// ============================================================================
// ----------------------------------------------------------------------------
// Do Paymento using TEF
SUB sTEFPayment(var tipo :A5, REF Valor, REF NumeroCuponFiscal, REF Resultado, VAR loTMDNUM :N9, ref _CancelType, ref DataFiscal, ref Horario)
	VAR TamBuffer : A6
	VAR Continua : A6
	VAR TrEmpChkName : A20
	VAR TEFFuncBloq : A2048 = "{[0;0]}"
	VAR TEFFuncBloqTMP : A2048
//	VAR TDEBSTART : N9 = TEF_TMED_DEB_START
//	VAR TDEBFINISH : N9 = TEF_TMED_DEB_FINISH
//	VAR TCREDSTART : N9 = TEF_TMED_CRED_START
//	VAR TCREDFINISH : N9 = TEF_TMED_CRED_FINISH
//	VAR TCHECK : N9 = TEF_TMED_CHECK

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sTEFPayment(",tipo, ", ", Valor, ", ", NumeroCuponFiscal, ", ", Resultado, ", ", loTMDNUM , ", ", _CancelType, ", ", DataFiscal, ", ", Horario, ")"
	CALL WriteLogFile(gbl_LogText, cFalse)

	gbl_TefPayment = 1
	
	_CancelType = -2

	FORMAT TrEmpChkName AS "{", TRIM(@tremp_chkname), "}"

	PROMPT "Conectando SITEF"
	CALL sConectSiTEFServer(Resultado, _CancelType)
	IF Resultado <> 0
		//ERRORMESSAGE "Nao foi possivel conectar com o servidor de TEF"
		FORMAT gbl_LogText AS @line{06}, " ", "TEF - Nao foi possivel conectar com o servidor de TEF"
		CALL WriteLogFile(gbl_LogText, ctrue)
		//CALL sFreeSiTEFDll()		
		CALL ExitSim(1, "Nao foi possivel conectar com o servidor de TEF")
	ENDIF
//	FORMAT TEFFuncBloq AS "{[", TRIM(TEF_FUNC_OUTROS)
//	IF loTMDNUM <> 0
//		IF loTMDNUM <> TCHECK
//			IF (loTMDNUM >= TDEBSTART) AND (loTMDNUM <= TDEBFINISH)
//				FORMAT TEFFuncBloqTMP AS TRIM(TEFFuncBloq), TRIM(TEF_FUNC_CREDITO), TRIM(TEF_FUNC_CHEQUE)
//				TEFFuncBloq = TEFFuncBloqTMP
//			ENDIF
//			IF (loTMDNUM >= TCREDSTART) AND (loTMDNUM <= TCREDFINISH)
//				FORMAT TEFFuncBloqTMP AS TRIM(TEFFuncBloq), TRIM(TEF_FUNC_DEBITO), TRIM(TEF_FUNC_CHEQUE)
//				TEFFuncBloq = TEFFuncBloqTMP
//			ENDIF
//		ELSE
//			FORMAT TEFFuncBloqTMP AS TRIM(TEFFuncBloq), TRIM(TEF_FUNC_CREDITO), TRIM(TEF_FUNC_DEBITO)
//			TEFFuncBloq = TEFFuncBloqTMP
//		ENDIF
//		TEFFuncBloq = MID(TEFFuncBloq, 1, LEN(TEFFuncBloq) - 1)
//	ENDIF
//	FORMAT TEFFuncBloqTMP AS TRIM(TEFFuncBloq), "]}"
//	TEFFuncBloq = TRIM(TEFFuncBloqTMP)

	FORMAT gbl_LogText AS @line{06}, " ", "TEF - Restricoes ", TEFFuncBloq
	CALL WriteLogFile(gbl_LogText, cfalse)

	PROMPT "Executando SITEF Interativo"
	DLLCALL hDLLSitef, IniciaFuncaoSiTefInterativoA (REF Resultado, tipo, ref Valor, REF NumeroCuponFiscal, REF DataFiscal, REF Horario, TrEmpChkName, TEFFuncBloq)
	IF Resultado <> 10000 // deve ser chamada a rotina de continuidade do processo
		CALL sEvaluateResultError(Resultado)
		//CALL sFreeSiTEFDll()
		
		CALL ExitSim(1, "Saindo do TEF. Cancel: ", _CancelType //MOD v6)
	ENDIF

	// Reset variables
	SETSTRING Comando , "0"
	SETSTRING TipoCampo , "0"
	SETSTRING gbl_TEF_TamMinimo , "0"
	SETSTRING gbl_TEF_TamMaximo , "0"
	SETSTRING Continua , "0"
	SETSTRING Buffer , " "
	SETSTRING field_100, " "
	SETSTRING field_131, " "
	SETSTRING field_132, " "

	// do things until the end
	FOREVER
		// adjust Buffer length to variable TamBuffer
		FORMAT TamBuffer AS (LEN(Buffer)){>06}

		IF (gbl_TEF_TamMinimo <> "000000" and gbl_TEF_TamMaximo <> "000000" AND Continua <> "1")
			IF MID(Buffer, 1, 1) = "{"
				IF (INSTR(1, Buffer, "}") - 2) < gbl_TEF_TamMinimo OR (INSTR(1, Buffer, "}") - 2) > gbl_TEF_TamMaximo
					INFOMESSAGE "TEF", "Entrada deve conter entre ", gbl_TEF_TamMinimo, " e ", gbl_TEF_TamMaximo, " caracteres"
					CALL WriteLogFile(gbl_LogText, cfalse)
					Comando = -1
				ENDIF
			ENDIF
		ELSEIF (Continua = "1")
			SETSTRING Comando , "0"
			SETSTRING TipoCampo , "0"
			SETSTRING gbl_TEF_TamMinimo , "0"
			SETSTRING gbl_TEF_TamMaximo , "0"
			SETSTRING Buffer , " "
			SETSTRING field_100, " "
			SETSTRING field_131, " "
			SETSTRING field_132, " "
		ENDIF

		
		FORMAT gbl_LogText AS @line{06}, " ", "TEF - BeforeCMD - Buf[", MID(Buffer, 1, 200), "]"
		CALL WriteLogFile(gbl_LogText, ctrue)
		FORMAT gbl_LogText AS @line{06}, " ", "TEF - BeforeCMD - Result[", Resultado, "]Cmd[", Comando, \
		"]TpCmpo[", TipoCampo, "]TMin[", gbl_TEF_TamMinimo, "]TMax[", gbl_TEF_TamMaximo , \
		"]Cont[", Continua, "]"
		CALL WriteLogFile(gbl_LogText, ctrue)
	
	
		FORMAT gbl_LogText AS @line{06}, " ", "TEF - BeforeCMD - Buf[", MID(Buffer, 1, 200), "]"
		CALL WriteLogFile(gbl_LogText, cfalse)
		

		DLLCALL hDLLSitef, ContinuaFuncaoSiTefInterativoA(REF Resultado, REF Comando, REF TipoCampo, REF gbl_TEF_TamMinimo, REF gbl_TEF_TamMaximo, REF Buffer, REF TamBuffer, REF Continua)
		//SETSTRING Continua , "0"

		// errormessage "recepcao ", @minute, @second
		FORMAT gbl_LogText AS @line{06}, " ", "TEF - AfterCMD - Result[", Resultado, "]Cmd[", Comando, \
		"]TpCmpo[", TipoCampo, "]TMin[", gbl_TEF_TamMinimo, "]TMax[", gbl_TEF_TamMaximo , \
		"]Cont[", Continua, "]"
		
		CALL WriteLogFile(gbl_LogText, ctrue)
				
		FORMAT gbl_LogText AS @line{06}, " ", "TEF - AfterCMD - Buf=[", Mid(Buffer, 1, 200), "]"
		
		CALL WriteLogFile(gbl_LogText, ctrue)
		
		// clean the variables TEF Simbols
		buffer = Trim(Buffer)
		IF MID(Buffer, 1, 1) = "{"
			MID(Buffer, 1, 1) = " "
			MID(Buffer, INSTR(1, Buffer, "}"), 1) = " "
		ENDIF

		// IF Resultado <> 10000, the show must go on!
		IF Resultado <> 10000
			IF Resultado <> 0
				FORMAT gbl_LogText AS @line{06}, " ", "TEF - ERRO; Resultado = ", Resultado
				CALL WriteLogFile(gbl_LogText, ctrue)
			ENDIF
			BREAK
		ENDIF
		CALL sEvaluateComando(_CancelType)
	ENDFOR

	IF Resultado <> 0
		// encerra transacao
		IF Resultado <> 10000 // deve ser chamada a rotina de continuidade do processo
			CALL sEvaluateResultError(Resultado)
			//CALL sFreeSiTEFDll()			
			CALL ExitSim(1, "Transacao TEF Encerrada")
			FORMAT nValueTef AS nValueTef + Valor
		ENDIF
	ENDIF
ENDSUB
// ----------------------------------------------------------------------------
// do administrative things in TEF
SUB sTEFAdminstrative(REF Valor, REF NumeroCuponFiscal, REF Resultado, ref _CancelType,ref DataFiscal, ref Horario)
	//	VAR DataFiscal : A8 //AAAAMMDD
	//	VAR Horario : A6 //HHMMSS
	VAR TamBuffer : A6
	VAR Continua : A6
	VAR TrEmpChkName : A20
	VAR locStatusCMD : N1

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sTEFAdminstrative(", Valor, ", ", NumeroCuponFiscal, ", ", Resultado, ", ", _CancelType,", ", DataFiscal, ", ", Horario, ")"
	CALL WriteLogFile(gbl_LogText, cFalse)	

	cleararray TEFVoucherBuffer1
	TEFVoucher1LineCnt = 0
	cleararray TEFVoucherBuffer2
	TEFVoucher2LineCnt = 0

	FORMAT TrEmpChkName AS "{", TRIM(@tremp_chkname), "}"

	CALL sConectSiTEFServer(Resultado, _CancelType)
	IF Resultado <> 0
		//CALL sFreeSiTEFDll()
		CALL ExitSim(1, "Erro Concectando ao TEF")
	ENDIF

	FORMAT DataFiscal AS "20", @year{>02}, @month{>02}, @day{>02}
	FORMAT Horario AS @hour{>02}, @minute{>02}, @second{>02}

	DLLCALL hDLLSitef, IniciaFuncaoSiTefInterativoA(REF Resultado, "110", Valor, REF NumeroCuponFiscal, REF DataFiscal, REF Horario, TrEmpChkName, "")
	IF Resultado <> 10000 // deve ser chamada a rotina de continuidade do processo
		CALL sEvaluateResultError(Resultado)
		//CALL sFreeSiTEFDll()
		CALL ExitSim(1, "Transacao TEF Interrompida")
	ENDIF

	// Reset variables
	SETSTRING Comando , "0"
	SETSTRING TipoCampo , "0"
	SETSTRING gbl_TEF_TamMinimo , "0"
	SETSTRING gbl_TEF_TamMaximo , "0"
	SETSTRING Continua , "0"
	SETSTRING Buffer , " "
	SETSTRING field_100, " "
	SETSTRING field_131, " "
	SETSTRING field_132, " "

	// do things until the end
	FOREVER
		// adjust Buffer length to variable TamBuffer
		FORMAT TamBuffer AS (LEN(Buffer)){>06}

		IF (gbl_TEF_TamMinimo <> "000000" and gbl_TEF_TamMaximo <> "000000" AND Continua <> "1")
			IF MID(Buffer, 1, 1) = "{"
				IF (INSTR(1, Buffer, "}") - 2) < gbl_TEF_TamMinimo OR (INSTR(1, Buffer, "}") - 2) > gbl_TEF_TamMaximo
					INFOMESSAGE "TEF", "Entrada deve conter entre ", gbl_TEF_TamMinimo, " e ", gbl_TEF_TamMaximo, " caracteres"
					CALL WriteLogFile(gbl_LogText, cfalse)
					Comando = -1
				ENDIF
			ENDIF
		ELSEIF (Continua = "1")
			SETSTRING Comando , "0"
			SETSTRING TipoCampo , "0"
			SETSTRING gbl_TEF_TamMinimo , "0"
			SETSTRING gbl_TEF_TamMaximo , "0"
			SETSTRING Buffer , " "
			SETSTRING field_100, " "
			SETSTRING field_131, " "
			SETSTRING field_132, " "
		ENDIF

//		if ForceTEFLog = ctrue
//			FORMAT gbl_LogText AS @line{06}, " ", "TEF - BeFOReCMD - Buf[", MID(Buffer, 1, 200), "]"
//			CALL WriteLogFile(gbl_LogText, ctrue)
//		endif
//		IF LOG_INTERFACE = ctrue
//			FORMAT gbl_LogText AS @line{06}, " ", "TEF - BeFOReCMD - Buf[", MID(Buffer, 1, 200), "]"
//			CALL WriteLogFile(gbl_LogText, cfalse)
//		ENDIF

		DLLCALL hDLLSitef, ContinuaFuncaoSiTefInterativoA(REF Resultado, REF Comando, REF TipoCampo, REF gbl_TEF_TamMinimo, REF gbl_TEF_TamMaximo, REF Buffer, REF TamBuffer, REF Continua)
		SETSTRING Continua , "0"

//		FORMAT gbl_LogText AS @line{06}, " ", "TEF - AfterCMD - Result[", Resultado, "]Cmd[", Comando, \
//		"]TpCmpo[", TipoCampo, "]TMin[", gbl_TEF_TamMinimo, "]TMax[", gbl_TEF_TamMaximo , \
//		"]Cont[", Continua, "]"
////		if ForceTEFLog = ctrue
////			CALL WriteLogFile(gbl_LogText, ctrue)
////		endif
//		CALL WriteLogFile(gbl_LogText, cfalse)
//		FORMAT gbl_LogText AS @line{06}, " ", "TEF - AfterCMD - Buf=[", Mid(Buffer, 1, 200), "]"
//		if ForceTEFLog = ctrue
//			CALL WriteLogFile(gbl_LogText, ctrue)
//		endif
//		CALL WriteLogFile(gbl_LogText, cfalse)

		// clean the variables TEF Simbols
		buffer = Trim(Buffer)
		IF MID(Buffer, 1, 1) = "{"
			MID(Buffer, 1, 1) = " "
			MID(Buffer, INSTR(1, Buffer, "}"), 1) = " "
		ENDIF

		// IF Resultado <> 10000, the show must go on!
		IF Resultado <> 10000
			IF Resultado <> 0
				FORMAT gbl_LogText AS @line{06}, " ", "TEF - ERRO; Resultado = ", Resultado
				CALL WriteLogFile(gbl_LogText, ctrue)
			ENDIF
			BREAK
		ENDIF
		CALL sEvaluateComando(_CancelType)
	ENDFOR

	IF Resultado <> 0
		// encerra transacao
		IF Resultado <> 10000 // deve ser chamada a rotina de continuidade do processo
			CALL sEvaluateResultError(Resultado)
			//CALL sFreeSiTEFDll()
			CALL ExitSim(1, "Transacao Tef Interrompida")
		ENDIF
	ENDIF
ENDSUB
// ----------------------------------------------------------------------------
SUB sEvaluateComando(ref __CancelType)
	VAR i : N9
	VAR j : N9
	// variables to TipoCampo = 121 and ListView
	VAR localHeader : A80
	VAR localListSize : A9
	VAR localListDetail[2000] : A80
	VAR Cmd31[8] : A10
	VAR tmp : A255
	VAR dtmp : $12
	VAR TMPVersion : A20
	VAR VersionPOS : $3
	VAR varInput : A255
	VAR Buffer_Tef_Mgs :A1024 //Alterado Cesar

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sEvaluateComando(", __CancelType, ")"
	CALL WriteLogFile(gbl_LogText, cFalse)

	IF Comando = 0
		IF TipoCampo = -1

		elseif TipoCampo = 506

		else
			CALL sEvaluateTipoCampo()
		ENDIF
		SETSTRING Buffer, " "
	ELSEIF Comando = 1
		PROMPT TRIM(Buffer)
		OldPromptMessage = TRIM(Buffer)
		IF LEN(TRIM(Buffer)) > 38
		ENDIF
		SETSTRING Buffer, " "
	ELSEIF Comando = 2
		CALL sShowClientTEFMsg(Buffer)
		SETSTRING Buffer, " "
	ELSEIF Comando = 3
		IF Len(Trim(Buffer)) > 0

			PROMPT TRIM(MID(Buffer,1,40))

			OldPromptMessage = TRIM(Buffer)
		ELSE
			//PROMPT "Aguardando SITEF..."
		ENDIF
		CALL sShowClientTEFMsg(Buffer)

		IF LEN(TRIM(Buffer)) > 38
			//INFOMESSAGE "TEF", trim(Buffer)
		ENDIF
		SETSTRING Buffer, " "
	ELSEIF Comando = 4
		//Texto que devera ser utilizado como cabecalho na apresentacao do menu (Comando 21)
		//infomessage "cabecalho do menu", trim(Buffer)
		//PROMPT "Aguardando SITEF..."
		IF TRIM(Buffer) = "{}"
			MenuTitle = " "
		ELSE
			IF MID(Buffer, 1, 1) = "{"
				MenuTitle = MID(Buffer, 2, (INSTR(2, Buffer, "}") - 2))
			ELSE
				MenuTitle = TRIM(Buffer)
			ENDIF
		ENDIF
		SETSTRING Buffer, " "
	ELSEIF Comando = 11
		//PROMPT ""
		//PROMPT "Aguardando SITEF..."
		SETSTRING Buffer, " "
	ELSEIF Comando = 12
		//infomessage Comando, "Deve remover a mensagem apresentada no visor do cliente"
		SETSTRING Buffer, " "
		FORMAT TMPVersion AS @version
		FORMAT TMPVersion AS MID(TMPVersion,1,3)
		VersionPOS = TMPVersion
		IF VersionPOS >= 4.10
			ClearRearArea
		ENDIF
	ELSEIF Comando = 13
		PROMPT ""
		//infomessage Comando, "Deve remover mensagem apresentada no visor do operador e do cliente"
		//PROMPT "Aguardando SITEF..."
		//CALL sShowClientTEFMsg("")
		FORMAT TMPVersion AS @version
		FORMAT TMPVersion AS MID(TMPVersion,1,3)
		VersionPOS = TMPVersion
		IF VersionPOS >= 4.10
			ClearRearArea
		ENDIF
		// verIFy IF Brazil.cfg is loaded; IF not, do it!
		// call sCheckInterface()
		SETSTRING Buffer, " "
	ELSEIF Comando = 14
		//infomessage Comando, "Deve limpar o texto utilizado como cabecalho na apresentacao do menu"
		MenuTitle = "" //limpo cabecalho menu!"
		//PROMPT "Aguardando SITEF..."
		SETSTRING Buffer, " "
	ELSEIF Comando = 20
		// Deve obter uma resposta do tipo SIM/NAO. No retorno o primeiro carater presente em Buffer deve conter 0 se confirma
		// e 1 se cancela
		FORMAT MenuTitle AS TRIM(Buffer)
		FORMAT Buffer AS "; [Sim(Enter)/Nao(Limpar)];"
		CALL sDisplayInputTEFYesOrNo(Buffer, varInput)
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF Comando = 21
		// Deve apresentar um menu de opcoes e permitir que o usuario selecione uma delas. Na chamada o parametro
		// Buffer contem as opcoes no FORMATo 1:texto;2:texto;...i:Texto;... A rotina da aplicacao deve apresentar as
		// opcoes da Forma que ela desejar (nao sendo necessario incluir os indices 1,2, ...) e apos a selecao feita
		// pelo usuario, retornar em Buffer o indice i escolhido pelo operador (em ASCII)
		CALL sDisplayInputTEF( Buffer, varInput)// "tecle a opcao desejada"
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Erro Processando TEF")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF Comando = 22
		// Deve aguardar uma tecla do operador. E utilizada quando se deseja que o operador seja avisado de alguma
		// mensagem apresentada na tela
		// remove the spaces and ['{'|'}']
		CALL sCleanVar(Buffer)

		// por os dados num array para apresentacao
		j = 1
		localListSize = 0
		FOR i = 1 TO LEN(Buffer)
			IF MID(Buffer, i, 1) = "\"
				localListSize = localListSize + 1
				localListDetail[localListSize] = MID(Buffer, j, i - j)
				j = i + 1
			ENDIF
		ENDFOR

		IF localListSize = 0
			localListSize = 1
			localListDetail[localListSize] = TRIM(Buffer)
		ENDIF

		localHeader = "Consulta"
		CALL sDisplayPageable(localHeader, localListSize, localListDetail[],__CancelType)

		//DISPLAYTEFBUFFER = cFalse
		SETSTRING Buffer, " "
	ELSEIF Comando = 23
		// Este comando indica que a rotina esta perguntando para a aplicacao se ele deseja interromper o processo de
		// coleta de dados ou nao. Esse codigo ocorre quando a DLL esta acessando algum periferico e permite que a
		// automacao interrompa esse acesso (por exemplo: aguardando a passagem de um cartao pela leitora ou a digitacao
		// de senha pelo cliente)
		// PROMPT "Aguardando SITEF..."
		CALL sDisplayContinuePeripheralsInput(varInput)
		IF trim(varInput) = "1"
			Continua = "0"
		ElSEIF trim(varInput) = "-1"
			Continua = "1"
			varInput = "1"
		ElSE
			Continua = "2"
		ENDIF
		//		errormessage "varInput:", varInput, "Continua :",Continua
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)

	ELSEIF Comando = 30
		// Deve ser lido um campo cujo tamanho esta entre gbl_TamMinimo e gbl_TamMaximo.
		// O campo lido deve ser devolvido em Buffer
		// PROMPT "Aguardando SITEF..."
		CALL sEvaluateTipoCampo()

	ELSEIF Comando = 31
		// Deve ser lido o numero de um cheque. A coleta pode ser feita via leitura de CMC-7 ou pela digitacao da primeira
		// linha do cheque. No retorno deve ser devolvido em Buffer ''0:'' ou ''1:'' seguido do numero coletado manualmente
		// ou pela leitura do CMC-7, respectivamente. Quando o numero FOR coletado manualmente o FORMATo e o seguinte:
		// Compensacao (3), Banco (3), Agencia (4), C1 (1), ContaCorrente (10), C2 (1), Numero do Cheque (6) e C3 (1),
		// nesta ordem. Notar que estes campos sao os que estao na parte superior de um cheque e na ordem apresentada.
		// Sugerimos que na coleta seja apresentada uma interface que permita ao operador identIFicar e digitar adequadamente
		// estas informacoes de forma que a consulta nao seja feita com dados errados, retornando como bom um cheque com problemas
		// COLOCAR SEM separadores - somente separador para 0 ou 1 inicial

		// ATENCAO - Automacao realizada para Leitor de Codigo de barras por teclado MaxyScan
		// Necessario seguintes configuracoes
		// - Envia ENTER final
		// - Envia Indicador de Erro (! para erro)
		// - Envia IdentIFicador (b para barras e c para CMC-7)
		Buffer = ""
		FOREVER
			INPUT Buffer, "Passe na leitora CMC-7 ou ENTER"
			IF Buffer = ""
				FOREVER
					i = 0 // use i as a flag

					// manual entry
					WINDOW 9, 70, "Dados do Cheque:"
					DISPLAY 1, 1, "Compensacao "
					DISPLAYINPUT 1, 15, Cmd31[1], ": "
					DISPLAY 2, 1, "Banco "
					DISPLAYINPUT 2, 15, Cmd31[2], ": "
					DISPLAY 3, 1, "Agencia "
					DISPLAYINPUT 3, 15, Cmd31[3], ": "
					DISPLAY 4, 1, "C1 "
					DISPLAYINPUT 4, 15, Cmd31[4], ": "
					DISPLAY 5, 1, "Conta Corr "
					DISPLAYINPUT 5, 15, Cmd31[5], ": "
					DISPLAY 6, 1, "C2 "
					DISPLAYINPUT 6, 15, Cmd31[6], ": "
					DISPLAY 7, 1, "Num Cheque "
					DISPLAYINPUT 7, 15, Cmd31[7], ": "
					DISPLAY 8, 1, "C3 "
					DISPLAYINPUT 8, 15, Cmd31[8], ": "
					WINDOWEDIT
					WINDOWCLOSE

					// check the variables - clean IF wrong
					IF LEN(TRIM(Cmd31[1])) < 1 OR LEN(TRIM(Cmd31[1])) > 3
						ERRORMESSAGE "Compensacao excede 3 caracteres!"
						Cmd31[1] = ""
						i = 1
					ENDIF
					IF LEN(TRIM(Cmd31[2])) < 1 OR LEN(TRIM(Cmd31[2])) > 4
						ERRORMESSAGE "Banco excede 4 caracteres!"
						Cmd31[2] = ""
						i = 1
					ENDIF
					IF LEN(TRIM(Cmd31[3])) < 1 OR LEN(TRIM(Cmd31[3])) > 4
						ERRORMESSAGE "Agencia excede 4 caracteres!"
						Cmd31[3] = ""
						i = 1
					ENDIF
					IF LEN(TRIM(Cmd31[4])) < 1 OR LEN(TRIM(Cmd31[4])) > 1
						ERRORMESSAGE "c1 excede 1 caracteres!"
						Cmd31[4] = ""
						i = 1
					ENDIF
					IF LEN(TRIM(Cmd31[5])) < 1 OR LEN(TRIM(Cmd31[5])) > 10
						ERRORMESSAGE "Conta Corrente excede 10 caracteres!"
						Cmd31[5] = ""
						i = 1
					ENDIF
					IF LEN(TRIM(Cmd31[6])) < 1 OR LEN(TRIM(Cmd31[6])) > 1
						ERRORMESSAGE "c2 excede 1 caracteres!"
						Cmd31[6] = ""
						i = 1
					ENDIF
					IF LEN(TRIM(Cmd31[7])) < 1 OR LEN(TRIM(Cmd31[7])) > 7
						ERRORMESSAGE "Numero Cheque excede 7 caracteres!"
						Cmd31[7] = ""
						i = 1
					ENDIF
					IF LEN(TRIM(Cmd31[8])) < 1 OR LEN(TRIM(Cmd31[8])) > 1
						ERRORMESSAGE "c3 excede 1 caracteres!"
						Cmd31[8] = ""
						i = 1
					ENDIF

					// break the FOREVER IF all fields are ok
					IF i = 0
						BREAK
					ENDIF
				ENDFOR

				// adjust the variables
				IF LEN(Cmd31[1]) < 3
					CALL sFillCMC7Buffer (Cmd31[1], 3)
				ENDIF
				IF len(Cmd31[2]) < 3
					CALL sFillCMC7Buffer (Cmd31[2], 3)
				ENDIF
				IF LEN(Cmd31[3]) < 4
					CALL sFillCMC7Buffer (Cmd31[3], 4)
				ENDIF
				IF LEN(Cmd31[5]) < 10
					CALL sFillCMC7Buffer (Cmd31[5], 10)
				ENDIF
				IF LEN(Cmd31[7]) < 6
					CALL sFillCMC7Buffer (Cmd31[7], 6)
				ENDIF

				FORMAT Buffer AS "{0:", Cmd31[1]{>03}, Cmd31[2]{>03},Cmd31[3]{>04},Cmd31[4]{>01},Cmd31[5]{>10},Cmd31[6]{>01},Cmd31[7]{>06},Cmd31[8]{>01}, "}"
				CALL sFillBuffer(Buffer)
				BREAK
			ELSE
				IF MID(Buffer, 1, 1) <> "c"
					ERRORMESSAGE "Nao e um codigo CMC7"
				ELSE
					IF MID(Buffer, 2, 1) = "!"
						ERRORMESSAGE "Erro lendo CMC7"
					ELSE
						tmp = TRIM(Buffer)
						FORMAT Buffer AS "{1:", MID(tmp, 2, LEN(tmp) - 1), "}"
						CALL sFillBuffer(Buffer)
						BREAK
					ENDIF
				ENDIF
			ENDIF
		ENDFOR
	ELSEIF Comando = 34
		// Deve ser lido um campo monetario ou seja, aceita o delimitador de centavos e devolvido no parametro Buffer
		MenuTitle = TRIM(Buffer)
		CALL sDisplayInputTEF( Buffer, varInput) //"Digitar valor"
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Erro Processando TEF")
		ENDIF
		dtmp = trim(varInput)
		FORMAT Buffer AS "{", dtmp, "}"
		CALL sFillBuffer(Buffer)
	ELSEIF Comando = 35
		// Deve ser lido um codigo em barras ou o mesmo deve ser coletado manualmente.
		// No retorno Buffer deve conter '0:' ou '1:' seguido do codigo em barras coletado manualmente ou pela leitora,
		// respectivamente. Cabe ao aplicativo decidir se a coleta sera manual ou atraves de uma leitora.
		// Caso seja coleta manual, recomenda-se seguir o procedimento descrito na rotina ValidaCampoCodigoEmBarras de
		// Forma a tratar um codigo em barras da forma mais generica possivel, deixando o aplicativo de automacao independente
		// de futuras alteracoes que possam surgir nos FORMATos em barras
		INFOMESSAGE Comando, "Ler Cod de Barras"
		SETSTRING Buffer, " "
	ELSE
		FORMAT gbl_LogText AS @line{06}, " ", "TEF Retornou um comando invalido: ", Comando
		CALL WriteLogFile(gbl_LogText, ctrue)
		ERRORMESSAGE
		//CALL sFreeSiTEFDll()
		CALL ExitSim(1, "Transacao TEF Encerrada")
	ENDIF
ENDSUB

SUB sDisplayContinuePeripheralsInput(REF keypress_confirm)
		VAR data : A255
		VAR ask : A255
		VAR keypress :Key
		VAR string :A255

		if PromptForContinuePeripheralsInput = ctrue
			FORMAT TMPLOG AS @line{06}, " ", "SUB - sDisplayContinuePeripheralsInput(", keypress_confirm, ")"
			CALL WriteLogFile(TMPLOG, cTrue)

			FORMAT ask AS TRIM(string), "(SIM/NAO)"
			FORMAT string as "Continuar captura de informacoes?"
			FOREVER
				WINDOW 4, gbl_WndColSize[@wstype], string
				DISPLAY 1, 2, ask

				INPUTKEY keypress, data, Trim(OldPromptMessage)
				IF (keypress = @key_enter)
					keypress_confirm = 1
					WINDOWCLOSE
					break
				elseif keypress = @key_clear
					keypress_confirm = 0
					WINDOWCLOSE
					break
				ELSEIF keypress = Key(1, 131323) // Voltar a tela anterior
					keypress_confirm = -1
					WINDOWCLOSE
					Break
				ENDIF
			ENDFOR
		else
			keypress_confirm = 1
			return
		endif
ENDSUB

// ----------------------------------------------------------------------------
SUB sEvaluateTipoCampo()
	VAR i : N9
	VAR j : N9
	VAR tmp : A80
	VAR itmp : N9 = 0
	VAR itmp1 : N9 = 0
	VAR retries : N9 = 0
	VAR varInput : A255
	// variables to TipoCampo = 121 and ListView
	VAR localHeader : A80
	VAR localListSize : A9
	VAR localListDetail[2000] : A80
	VAR TEFPwd								: N15
	VAR TmpForceLineBreak			: N15
	VAR TmpLineBuffer					: A2048

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sEvaluateTipoCampo()"
	CALL WriteLogFile(gbl_LogText, cFalse)

	IF TipoCampo = -1
		// Nao existem inFORmacoes que podem/devem ser tratadas pela automacao
		FOREVER
			FORMAT MenuTitle AS TRIM(Buffer)
			CALL sDisplayInputTEF(Buffer, varInput)
			IF trim(varInput) = "-1"
				CALL ExitSim(1, "Transacao Tef Encerradea")
			ENDIF
			FORMAT Buffer AS "{", TRIM(varInput), "}"
			CALL sFillBuffer(Buffer)
			if Continua <> "1"
				IF (LEN(TRIM(varInput)) <= gbl_TEF_TamMaximo AND LEN(TRIM(varInput)) >= gbl_TEF_TamMinimo) OR @inputstatus = 0
					BREAK
				ELSE
					INFOMESSAGE "TEF", "Entrada deve conter entre ", gbl_TEF_TamMinimo, " e ", gbl_TEF_TamMaximo, " caracteres"
					CALL WriteLogFile(gbl_LogText, cfalse)
				endif
				IF @inputstatus = 0
					SETSTRING varInput, "0", gbl_TamMaximo
					Continua = "-1"
				ENDIF
			else
				SETSTRING varInput, "0", gbl_TamMaximo
				BREAK
			ENDIF
		ENDFOR
		FORMAT Buffer AS "{", trim(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 0
		// A rotina esta sendo chamada para indicar que acabou de coletar os dados da transacao e
		// ira iniciar a interacao com o SiTef para obter a autorizacao
		IF Comando = 30
			MenuTitle = TRIM(buffer)
			CALL sDisplayInputTEF( Buffer, varInput)
			IF trim(varInput) = "-1"
				CALL ExitSim(1, "Transacao TEF Encerrada")
			ENDIF
			FORMAT Buffer AS "{", TRIM(varInput), "}"
		ELSE
			FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		ENDIF
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo >= 10 AND TipoCampo <= 99
		// InFORma qual a opcao selecionada no menu de navegacao de transacoes seguindo a mesma
		// codIFicacao utilizada para definir as restricoes no pagamento descritas no item 5
		IF TipoCampo = 10
			// PROMPT "Consulta ou garantia de Cheque (tods os tipos)"
		ELSEIF TipoCampo = 11
			// PROMPT "Consulta Cheque Serasa/Associacao Comercial"
		ELSEIF TipoCampo = 12
			// PROMPT "Consulta Cheque Tecban"
		ELSEIF TipoCampo = 13
			// PROMPT " Telecheque Garantido Tecban"
		ELSEIF TipoCampo = 14
			// PROMPT " Garantia Cheque Papel Tecban"
		ELSEIF TipoCampo = 15
			// PROMPT " Cartao de debito (todas as combinacoes)"
		ELSEIF TipoCampo = 16
			// PROMPT " Cartao de debito a vista"
		ELSEIF TipoCampo = 17
			// PROMPT " Cartao de debito pre-datado"
		ELSEIF TipoCampo = 18
			// PROMPT " Cartao de debito parcelado"
		ELSEIF TipoCampo = 19
			// PROMPT " Cartao de debito CDC"
			// don't print buffer; just list it in the screen!
			DISPLAYTEFBUFFER = cTrue
		ELSEIF TipoCampo = 20
			// PROMPT " Cartao Refeicao Eletronico"
		ELSEIF TipoCampo = 25
			// PROMPT " Cartao de credito (todas as combinacoes)"
		ELSEIF TipoCampo = 26
			// PROMPT " Cartao de credito a vista"
		ELSEIF TipoCampo = 27
			// PROMPT " Cartao de credito parcelado com financiamento do estabelecimento"
		ELSEIF TipoCampo = 28
			// PROMPT " Cartao de credito parcelado com financiamento da administradora"
		ELSEIF TipoCampo = 29
			// PROMPT " Cartao de credito digitado"
		ELSEIF TipoCampo = 30
			// PROMPT " Cartao de credito magnetico"
		ELSEIF TipoCampo = 31
			// PROMPT " Pre-autorizacao"
		ELSEIF TipoCampo = 32
			// PROMPT " Cartao Fininvest"
		ELSEIF TipoCampo = 33
			// PROMPT " Saque com cartao Fininvest"
			// don't print buffer; just list it in the screen!
			DISPLAYTEFBUFFER = cTrue
		ELSEIF TipoCampo = 34
			// PROMPT " Cartao de Credito Pro-rata a vista"
		ELSEIF TipoCampo = 35
			// PROMPT " Cartao de Credito Pro-rata parcelada"
		ELSEIF TipoCampo = 36
			// PROMPT " Consulta parcelas no Cartao de Credito"
			// don't print buffer; just list it in the screen!
			DISPLAYTEFBUFFER = cTrue
		ELSEIF TipoCampo = 40
			// PROMPT " Cancelamento de transacao com cartao de credito ou debito"
		ELSEIF TipoCampo = 60
			// PROMPT " Recarga de celular com Dinheiro"
		ELSEIF TipoCampo = 61
			// PROMPT " Recarga de celular com Cheque"
		ELSEIF TipoCampo = 62
			// PROMPT " Recarga de celular com cartao de debito a vista"
		ELSEIF TipoCampo = 63
			// PROMPT " Recarga de celular com cartao de credito a vista"
		ENDIF
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 100
		// Contem a modalidade de pagamento no FORMATo xxnn xx corresponde ao grupo da modalidade
		// e nn ao SUB-grupo. Vide tabela no final deste documento descrevendo os possiveis valores de xx e nn.
		call sCleanVar(Buffer)
		FORMAT field_100 AS mid(Buffer, 1, 2)
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 101
		// Contem o texto real da modalidade de pagamento que pode ser memorizado pela aplicacao
		// caso exista essa necessidade. Descreve por extenso o par xxnn FORnecido em 100
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 102
		// Contem o texto descritivo da modalidade de pagamento que deve ser impresso no cupon fiscal (p/ex: T.E.F., Cheque, etc...)
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 105
		// Contem a data e hora da transacao no FORMATo AAAAMMDDHHMMSS
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 110
		// Contem a modalidade de cancelamento no FORMATo xxnn xx corresponde ao grupo da modalidade
		// e nn ao SUB-grupo. Vide tabela no final deste documento descrevendo os possiveis valores de xx e nn.
		// Retorna quando uma transacao FOR cancelada
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 111
		// Contem o texto real da modalidade de cancelamento que pode ser memorizado pela aplicacao
		// caso exista essa necessidade. Descreve por extenso o par xxnn FORnecido em 110
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 112
		// Contem dados adicionais relativos ao cartao utilizado na operacao de Tef.
		// O FORMATo deste campo e uma seqüência de codigos separados por ponto-e-virgula.
		// Por enquanto o unico codigo que retorna e a palavra EE para indicar que o cartao utilizado foi um
		// EasyEntry ou EMV para indicar um cartao EMV. Caso tenha sido um cartao normal,
		// esse tipo de campo nao e retornado para a aplicacao
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 120
		// Buffer contem a linha de autenticacao do cheque para ser impresso no verso do mesmo
		CALL sCleanVar(Buffer)
		INFOMESSAGE "Anotar no verso do cheque:", CHR(10), TRIM(Buffer)
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 121
		// Buffer contem a primeira via do comprovante de pagamento (via do cliente) a ser impressa na ECF.
		// Essa via, quando possivel, e reduzida de forma a ocupar poucas linhas na impressora.
		// Pode ser um comprovante de venda ou administrativo

		// remove the spaces and ['{'|'}']
		//		CALL sCleanVar(Buffer)

		FORMAT gbl_LogText AS @line{06}, " ", "TEF Tipo Campo 121 -> Buffer=", Buffer
		CALL WriteLogFile(gbl_LogText, cfalse)
		// Mount Array with returns
		// changed
		//		cleararray TEFVoucherBuffer1
		//		TEFVoucher1LineCnt = 0
		i = 0
		VAR cntLines : N7 = 0
		VAR StartPos : N7 = 1
		if TEFVoucher1LineCnt <> 0
			TEFVoucher1LineCnt=TEFVoucher1LineCnt+1
			TEFVoucherBuffer1[TEFVoucher1LineCnt] = "                                                "
			TEFVoucher1LineCnt=TEFVoucher1LineCnt+1
			TEFVoucherBuffer1[TEFVoucher1LineCnt] = "                                                "
			TEFVoucher1LineCnt=TEFVoucher1LineCnt+1
			TEFVoucherBuffer1[TEFVoucher1LineCnt] = "                                                "
		endif
		FOREVER
			i = instr(StartPos, Buffer, "\" )
			IF i = 0
				IF Len(mid(Buffer, StartPos, (len(Buffer) - StartPos + 1))) > 0
					format TmpLineBuffer as mid(Buffer, StartPos, (len(Buffer) - StartPos + 1))
					if Len(TmpLineBuffer) > 48
						forever
							if Len(TmpLineBuffer) = 0
								break
							else
								TEFVoucher1LineCnt=TEFVoucher1LineCnt+1
								TEFVoucherBuffer1[TEFVoucher1LineCnt] = mid(TmpLineBuffer, 1, 48)
								TmpLineBuffer = mid(TmpLineBuffer,49 ,Len(TmpLineBuffer) -48)
								FORMAT gbl_LogText AS @line{06}, " ", "TEF TEFVoucherBuffer1[", TEFVoucher1LineCnt, "]=", TEFVoucherBuffer1[TEFVoucher1LineCnt], "i=", i, " StartPos=", StartPos
								CALL WriteLogFile(gbl_LogText, cfalse)
							endif
						endfor
					else
						TEFVoucher1LineCnt=TEFVoucher1LineCnt+1
						TEFVoucherBuffer1[TEFVoucher1LineCnt] = TmpLineBuffer
						
						FORMAT gbl_LogText AS @line{06}, " ", "TEF TEFVoucherBuffer1[", TEFVoucher1LineCnt, "]=", TEFVoucherBuffer1[TEFVoucher1LineCnt], "i=", i, " StartPos=", StartPos
						CALL WriteLogFile(gbl_LogText, cfalse)
					endif
				ENDIF
				break
			ELSE
				TEFVoucher1LineCnt=TEFVoucher1LineCnt+1
				TEFVoucherBuffer1[TEFVoucher1LineCnt] = mid(Buffer, StartPos,(i-StartPos))
				
				FORMAT gbl_LogText AS @line{06}, " ", "TEF TEFVoucherBuffer1[", TEFVoucher1LineCnt, "]=", TEFVoucherBuffer1[TEFVoucher1LineCnt], "i=", i, " StartPos=", StartPos
				CALL WriteLogFile(gbl_LogText, ctrue)
								
				StartPos = i + 1
			ENDIF
		ENDFOR
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 122
		// Buffer contem a segunda via do comprovante de pagamento (via do caixa) a ser impresso na ECF.
		// Pode ser um comprovante de venda ou administrativo
		// suprimido pois existem clientes com papel carbonado, que necessita apenas impressao de 1 via
		// remove the spaces and ['{'|'}']
		//		CALL sCleanVar(Buffer)
		FORMAT gbl_LogText AS @line{06}, " ", "TEF Tipo Campo 122 -> Buffer=", Buffer
		CALL WriteLogFile(gbl_LogText, cfalse)
		//		cleararray TEFVoucherBuffer2
		//		TEFVoucher2LineCnt = 0
		i = 0
		VAR cntLines : N03 = 0
		VAR StartPos : N07 = 1
		if TEFVoucher2LineCnt <> 0
			TEFVoucher2LineCnt=TEFVoucher2LineCnt+1
			TEFVoucherBuffer2[TEFVoucher2LineCnt] = "                                                "
			TEFVoucher2LineCnt=TEFVoucher2LineCnt+1
			TEFVoucherBuffer2[TEFVoucher2LineCnt] = "                                                "
			TEFVoucher2LineCnt=TEFVoucher2LineCnt+1
			TEFVoucherBuffer2[TEFVoucher2LineCnt] = "                                                "
		endif
		FOREVER
			i = instr(StartPos, Buffer, "\" )
			IF i = 0
				IF Len(mid(Buffer, StartPos, (len(Buffer) - StartPos + 1))) > 0
					format TmpLineBuffer as mid(Buffer, StartPos, (len(Buffer) - StartPos + 1))
					if Len(TmpLineBuffer) > 48
						forever
							if Len(TmpLineBuffer) = 0
								break
							else
								TEFVoucher2LineCnt=TEFVoucher2LineCnt+1
								TEFVoucherBuffer2[TEFVoucher2LineCnt] = mid(TmpLineBuffer, 1, 48)
								TmpLineBuffer = mid(TmpLineBuffer,49 ,Len(TmpLineBuffer) -48)
				
								FORMAT gbl_LogText AS @line{06}, " ", "TEF TEFVoucherBuffer2[", TEFVoucher2LineCnt, "]=", TEFVoucherBuffer2[TEFVoucher2LineCnt], "i=", i, " StartPos=", StartPos
								CALL WriteLogFile(gbl_LogText, ctrue)
								
							endif
						endfor
					else
						TEFVoucher2LineCnt=TEFVoucher2LineCnt+1
						TEFVoucherBuffer2[TEFVoucher2LineCnt] = TmpLineBuffer
				
						FORMAT gbl_LogText AS @line{06}, " ", "TEF TEFVoucherBuffer2[", TEFVoucher2LineCnt, "]=", TEFVoucherBuffer2[TEFVoucher2LineCnt], "i=", i, " StartPos=", StartPos
						CALL WriteLogFile(gbl_LogText, ctrue)
				
						FORMAT gbl_LogText AS @line{06}, " ", "TEF TEFVoucherBuffer1[", TEFVoucher2LineCnt, "]=", TEFVoucherBuffer2[TEFVoucher2LineCnt], "i=", i, " StartPos=", StartPos
						CALL WriteLogFile(gbl_LogText, cfalse)
					endif
				ENDIF
				break
			ELSE
				TEFVoucher2LineCnt=TEFVoucher2LineCnt+1
				TEFVoucherBuffer2[TEFVoucher2LineCnt] = mid(Buffer, StartPos,(i-StartPos))
				FORMAT gbl_LogText AS @line{06}, " ", "TEF TEFVoucherBuffer2[", TEFVoucher2LineCnt, "]=", TEFVoucherBuffer2[TEFVoucher2LineCnt]
				CALL WriteLogFile(gbl_LogText, cfalse)
				StartPos = i + 1
			ENDIF
		ENDFOR
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 130
		// Indica, na coleta, que o campo em questao e o valor do troco em dinheiro a ser devolvido para o cliente.
		// Na devolucao de resultado (Comando = 0) contem o valor efetivamente aprovado para o troco
		itmp = instr(1, Buffer, ",")
		mid(Buffer, itmp, 1) = "."
		IF Buffer > 0
			tnd_saque_ttl = TRIM(Buffer)
		ENDIF
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 131
		// Contem um indice que indica qual a instituicao que ira processar a transacao segundo a tabela
		// presente no final do documento (5 posicoes)
		call sCleanVar(Buffer)
		field_131 = Buffer
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 132
		// Contem um indice que indica qual o tipo do cartao quando esse tipo for identificavel,
		// segundo uma tabela a ser Fornecida (5 posicoes)
		call sCleanVar(Buffer)
		field_132 = Buffer
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 133
		// Contem o NSU do SiTef (6 posicoes)
		FORMAT TEFTMedInfo AS "NSU:", TRIM(Buffer)
		FORMAT TEFNSUinfo AS "NSU:", TRIM(Buffer)
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 134
		// Contem o NSU do Host autorizador (15 posicoes no maximo)
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 135
		// Contem o Codigo de Autorizacao para as transacoes de credito (15 posicoes no maximo)
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 136
		// Contem as 6 primeiras posicoes do cartao (bin)
		IF trim(TEFTMedInfo) <> ""
			FORMAT TEFTMedInfo AS TEFTMedInfo, "/", trim(Buffer)
		ENDIF
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 140
		// Numero de parcelas em venda parcelada
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 141
		// Data da parcela no FORMATo aaaammdd
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 142
		// Valor da parcela
		//(Os campos 141 e 142 sao chamados n vezes onde n = conteudo do campo 140)
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 145
		// Data do Pre-datado no FORMATo aaaammdd
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 146
		// A rotina esta sendo chamada para ler o Valor a ser cancelado. Caso o aplicativo de automacao possua esse valor,
		// pode apresenta-lo para o operador e permitir que ele confirme o valor antes de passa-lo devolvê-lo para a rotina.
		// Caso ele nao possua esse valor, deve lê-lo.
		//TEFREPRINTTRANSACTION = cFalse // doing a cancelation... not a reimpression!!!
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 150
		// Contem a Trilha 1, quando disponivel, obtida na funcao LeCartaoInterativo
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 151
		// Contem a Trilha 2, quando disponivel, obtida na funcao LeCartaoInterativo
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 153
		// Contem a senha do cliente capturada atraves da rotina LeSenhaInterativo
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 500
		// Indica que o campo em questao e o codigo do supervisor. A automacao, pode, se desejado,
		// validar os dados coletados, deixando o fluxo da transacao seguir normalmente caso seja um supervisor aceitavel
		// eu tenho que solicitar uma senha de administrador... ver com Fernando como pegar senhas do Micros
		// enviar a senha para a DLL da sw express
		//FORMAT Buffer as "{3700}"
		//CALL sFillBuffer(Buffer)
		call sCheckMicrosCode(TEFPwd)

		FORMAT Buffer as "{" , TEFPwd, "}"
		CALL sFillBuffer(Buffer)

	ELSEIF TipoCampo = 501
		// Tipo do Documento a ser consultado (0 - CPF, 1 - CGC)
		MenuTitle = TRIM(Buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 502
		// Numero do documento (CPF ou CGC)
		MenuTitle = TRIM(buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 503
		// Numero do Telefone
		MenuTitle = TRIM(buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 504
		// Taxa de Servico
		MenuTitle = trim(buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 505
		// Numero de Parcelas
		MenuTitle = TRIM(buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput) {> 02}, "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 506
		// Data do Pre-datado
		MenuTitle = TRIM(buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 507
		// Captura se a primeira parcela e a vista ou nao (0 - Primeira a vista, 1 - caso contrario)
		MenuTitle = TRIM(buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 508
		// Intervalo em dias entre parcelas
		MenuTitle = TRIM(buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 509
		// Captura se e mês fechado (0) ou nao (1)
		MenuTitle = TRIM(buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 510
		// Captura se e com (0) ou sem (1) garantia no pre-datado com cartao de debito
		MenuTitle = TRIM(buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 511
		// Numero de Parcelas CDC
		MenuTitle = TRIM(buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 512
		// Numero do Cartao de Credito Digitado
		MenuTitle = TRIM(buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 513
		// Data de vencimento do Cartao
		MenuTitle = TRIM(Buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ELSE
			//Prompt "Aguardando SITEF..."
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 514
		// Codigo de seguranca do Cartao
		MenuTitle = TRIM(buffer)
		call sDisplayInputTEFSecureCode(Buffer, varInput)
		//CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 515
		// Data da transacao a ser cancelada (DDMMAAAA) ou a ser re-impressa
		MenuTitle = TRIM(buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 516
		// Numero do documento a ser cancelado ou a ser re-impresso
		//TEFREPRINTTRANSACTION = cTrue
		MenuTitle = TRIM(buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 517
		// A rotina esta sendo chamada para ler o Numero do cheque segundo o descrito no tipo de comando correspondente ao valor 31
		MenuTitle = TRIM(buffer)
		FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 522
		// A rotina esta sendo chamada para ler o DDD de um telefone com ate 4 digitos
		MenuTitle = TRIM(buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", trim(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 523
		// A rotina esta sendo chamada para ler o Numero do Telefone
		MenuTitle = TRIM(buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
		// =================== CodIFicacao dos campos utilizados na Recarga de Pre-pago
	ELSEIF TipoCampo = 590
		// Nome da Operadora de Celular selecionada para a operacao
		MenuTitle = TRIM(buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 591
		// Valor selecionado para a recarga
		MenuTitle = TRIM(buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 592
		// DDD + Numero do celular a ser recarregado
		MenuTitle = TRIM(buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 593
		// Digito(s) verIFicadores
		MenuTitle = TRIM(buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSEIF TipoCampo = 594
		// Cep da localidade onde esta o terminal no qual a operacao esta sendo feita
		MenuTitle = TRIM(buffer)
		CALL sDisplayInputTEF( Buffer, varInput)
		IF trim(varInput) = "-1"
			CALL ExitSim(1, "Transacao TEF Encerrada")
		ENDIF
		FORMAT Buffer AS "{", TRIM(varInput), "}"
		CALL sFillBuffer(Buffer)
	ELSE // TipoCampo nao esta listado acima
		// O aplicativo de automacao deve ignorar aqueles campos que nao desejar/nao
		// souber tratar uma vez que, em versoes futuras da CliSiTef, novos campos
		// poderao ser disponibilizados. Notar que a forma correta de ignorar estes campos
		// e executar a funcao definida em ProximoComando ou simplesmente ignorar o dado retornado
		// para a aplicacao quando ProximoComando FOR 0.
		IF Comando = 30
			MenuTitle = TRIM(buffer)
			CALL sDisplayInputTEF( Buffer, varInput)
			IF trim(varInput) = "-1"
				CALL ExitSim(1, "Transacao TEF Encerrada")
			ENDIF
			FORMAT Buffer AS "{", TRIM(varInput), "}"
		ELSE
			FORMAT Buffer AS "{", TRIM(TipoCampo), "}"
		ENDIF
		CALL sFillBuffer(Buffer)
	ENDIF
ENDSUB

SUB sDisplayInputTEF(REF _buffer, ref InputAnswer)
	VAR Max_int_VAR :N06
	
	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sDisplayInputTEF(", _buffer, ", ", InputAnswer, ")"
	CALL WriteLogFile(gbl_LogText, cFalse)
	
	VAR NewInputAnswer :A255

	call sDisplayInputTEFWithVarType(_buffer, NewInputAnswer)
	InputAnswer = NewInputAnswer

ENDSUB
// ----------------------------------------------------------------------------
SUB sDisplayInputTEFWithVarType(REF _buffer, ref InputAnswer)
	VAR maxRow : N5 = 11
	VAR maxCol : N5 = 50 //67//70
	VAR bufferLength : N5
	VAR Row : N5 = 1
	VAR Column : N5 = 2
	VAR SeparatorPosition : N5
	VAR localkey : KEY
	VAR titlearray[10] : A255
	VAR i : N5
	VAR tmpline : A255
	VAR tmpcolcount : N5 = 1
	VAR keypress_confirm : KEY

	VAR Max_int :N06
	VAR Min_int :N06

	Max_int = gbl_TEF_TamMaximo
	Min_int = gbl_TEF_TamMinimo

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sDisplayInputTEFWithVarType(", _buffer, ", ", InputAnswer, ")"
	CALL WriteLogFile(gbl_LogText, cFalse)

	Continua = "0"

	VAR MenuTitle2 : A255

	format MenuTitle2 as Trim(MenuTitle)
	format MenuTitle as Trim(MenuTitle)

	IF instr(1, MenuTitle, "\") > 0
		split MenuTitle, "\", titlearray[1], titlearray[2], titlearray[3], titlearray[4], titlearray[5], \
		titlearray[6], titlearray[7], titlearray[8], titlearray[9], titlearray[10]
		MenuTitle = " "
	ENDIF

	FOREVER
		WINDOW maxRow+1, maxCol, MenuTitle
		IF len(trim(titlearray[1])) > 0
			FOR i = 1 to 10
				IF trim(titlearray[i]) <> ""
					DISPLAY Row, 1, trim(titlearray[i]){=50}//{=68}
					Row = Row + 1
				ELSE
					break
				ENDIF
			ENDFOR
			DISPLAY Row, 1, " "
			Row = Row + 1
		ENDIF
		WHILE Row < maxRow
			SeparatorPosition = INSTR(Column, _buffer, ";")
			IF SeparatorPosition = 0
				BREAK
			ENDIF
			SeparatorPosition = SeparatorPosition - Column
			tmpline = MID(_buffer, Column, SeparatorPosition)

			IF instr(1, tmpline, chr(10)) > 0
				FOREVER
					IF instr(tmpcolcount, tmpline, chr(10)) = 0
						DISPLAY Row, 1, mid(tmpline, tmpcolcount, len(tmpline) - tmpcolcount)
					ELSE
						DISPLAY Row, 1, mid(tmpline, tmpcolcount, instr(tmpcolcount, tmpline, chr(10) - tmpcolcount)
					ENDIF
					tmpcolcount = instr(tmpcolcount, tmpline, chr(10)) + 1
					Row = Row + 1

					IF tmpcolcount = 1
						break
					ENDIF
				ENDFOR
			ELSE
				DISPLAY Row, 1, tmpline
			ENDIF

			Column = Column + SeparatorPosition + 1
			Row = Row + 1
		ENDWHILE

		INPUTKEY localkey, InputAnswer, MenuTitle
		WINDOWCLOSE

		IF localkey = @key_enter
			IF (LEN(InputAnswer) < Min_Int) or (LEN(InputAnswer) > Max_int)
				Errormessage "Tamanho esperado entre ",Min_Int, " e ", Max_int
				loadKybdMacro Key(1,131083)  // delete
				Row = 1
				Column = 2
				tmpcolcount = 1
			ELSEIF (LEN(InputAnswer) >= Min_Int) and (LEN(InputAnswer) <= Max_int)
				Continua = "0"
				BREAK
			ELSE
				Row = 1
				Column = 2
				tmpcolcount = 1
			ENDIF
		ELSEIF localkey = @key_clear
			call sYesOrNo(keypress_confirm, "Deseja cancelar?")
			IF keypress_confirm = @key_enter
				InputAnswer = -1
				break
			ENDIF
		ELSEIF localkey = Key(1, 131323) // Voltar a tela anterior
			Continua = "1"
			Break
		ELSE
			IF (LEN(InputAnswer) < Min_Int) or (LEN(InputAnswer) > Max_int)
				Errormessage "Tamanho esperado entre ",Min_Int, " e ", Max_int
				loadKybdMacro Key(1,131083)  // delete
				Row = 1
				Column = 2
				tmpcolcount = 1
			ELSEIF (LEN(InputAnswer) >= Min_Int) and (LEN(InputAnswer) <= Max_int)
				BREAK
				Continua = "0"
			ELSE
				Row = 1
				Column = 2
				tmpcolcount = 1
			ENDIF

		ENDIF
	ENDFOR
ENDSUB
// ----------------------------------------------------------------------------
SUB sDisplayInputTEFSecureCode(REF _buffer, ref InputAnswer)
	VAR maxRow : N5 = 11
	VAR maxCol : N5 = 70
	VAR bufferLength : N5
	VAR Row : N5 = 1
	VAR Column : N5 = 2
	VAR SeparatorPosition : N5
	VAR localkey : KEY
	VAR titlearray[10] : A255
	VAR i : N5
	VAR tmpline : A255
	VAR tmpcolcount : N5 = 1
	VAR keypress_confirm : KEY

	VAR Max_int :N06
	VAR Min_int :N06

	Max_int = gbl_TEF_TamMaximo
	Min_int = gbl_TEF_TamMinimo

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sDisplayInputTEFSecureCode(", _buffer, ", ", InputAnswer, ")"
	CALL WriteLogFile(gbl_LogText, cFalse)

	Continua = "0"

	VAR MenuTitle2 : A255

	format MenuTitle2 as Trim(MenuTitle)
	format MenuTitle as Trim(MenuTitle)

	IF instr(1, MenuTitle, "\") > 0
		split MenuTitle, "\", titlearray[1], titlearray[2], titlearray[3], titlearray[4], titlearray[5], \
		titlearray[6], titlearray[7], titlearray[8], titlearray[9], titlearray[10]
		MenuTitle = " "
	ENDIF

	FOREVER
		WINDOW maxRow+1, maxCol, MenuTitle
		IF len(trim(titlearray[1])) > 0
			FOR i = 1 to 10
				IF trim(titlearray[i]) <> ""
					DISPLAY Row, 1, trim(titlearray[i]){=63}//{=68}
					Row = Row + 1
				ELSE
					break
				ENDIF
			ENDFOR
			DISPLAY Row, 1, " "
			Row = Row + 1
		ENDIF
		WHILE Row < maxRow
			SeparatorPosition = INSTR(Column, _buffer, ";")
			IF SeparatorPosition = 0
				BREAK
			ENDIF
			SeparatorPosition = SeparatorPosition - Column
			tmpline = MID(_buffer, Column, SeparatorPosition)

			IF instr(1, tmpline, chr(10)) > 0
				FOREVER
					IF instr(tmpcolcount, tmpline, chr(10)) = 0
						DISPLAY Row, 1, mid(tmpline, tmpcolcount, len(tmpline) - tmpcolcount)
					ELSE
						DISPLAY Row, 1, mid(tmpline, tmpcolcount, instr(tmpcolcount, tmpline, chr(10) - tmpcolcount)
					ENDIF
					tmpcolcount = instr(tmpcolcount, tmpline, chr(10)) + 1
					Row = Row + 1

					IF tmpcolcount = 1
						break
					ENDIF
				ENDFOR
			ELSE
				DISPLAY Row, 1, tmpline
			ENDIF

			Column = Column + SeparatorPosition + 1
			Row = Row + 1
		ENDWHILE

		INPUTKEY localkey, InputAnswer, MenuTitle
		WINDOWCLOSE

		IF localkey = @key_enter
			IF (LEN(InputAnswer) < Min_Int) or (LEN(InputAnswer) > Max_int)
				Errormessage "Tamanho esperado entre ",Min_Int, " e ", Max_int
				loadKybdMacro Key(1,131083)  // delete
				Row = 1
				Column = 2
				tmpcolcount = 1
			ELSEIF (LEN(InputAnswer) >= Min_Int) and (LEN(InputAnswer) <= Max_int)
				Continua = "0"
				BREAK
			ELSE
				Row = 1
				Column = 2
				tmpcolcount = 1
			ENDIF
		ELSEIF localkey = @key_clear
			call sYesOrNo(keypress_confirm, "Deseja cancelar?")
			IF keypress_confirm = @key_enter
				InputAnswer = -1
				break
			ENDIF
		ELSEIF localkey = Key(1, 131323) // Voltar a tela anterior
			Continua = "1"
			Break
		ELSE
			IF (LEN(InputAnswer) < Min_Int) or (LEN(InputAnswer) > Max_int)
				Errormessage "Tamanho esperado entre ",Min_Int, " e ", Max_int
				loadKybdMacro Key(1,131083)  // delete
				Row = 1
				Column = 2
				tmpcolcount = 1
			ELSEIF (LEN(InputAnswer) >= Min_Int) and (LEN(InputAnswer) <= Max_int)
				Continua = "0"
				BREAK
			ELSE
				Row = 1
				Column = 2
				tmpcolcount = 1
			ENDIF

		ENDIF
	ENDFOR
ENDSUB
// ----------------------------------------------------------------------------
SUB sDisplayInputTEFYesOrNo(REF _buffer, ref InputAnswer)
	VAR maxRow : N5 = 12
	VAR maxCol : N5 = 65//70
	VAR bufferLength : N5
	VAR Row : N5 = 1
	VAR Column : N5 = 2
	VAR SeparatorPosition : N5
	VAR localkey : KEY
	VAR titlearray[10] : A255
	VAR i : N5
	VAR tmpline : A255
	VAR tmpcolcount : N5 = 1
	VAR keypress_confirm : KEY
	VAR LocalMenuTitle :A65

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sDisplayInputTEFYesOrNo(", _buffer, ", ", InputAnswer, ")"
	CALL WriteLogFile(gbl_LogText, cFalse)

	IF instr(1, MenuTitle, "\") > 0
		split MenuTitle, "\", titlearray[1], titlearray[2], titlearray[3], titlearray[4], titlearray[5], \
		titlearray[6], titlearray[7], titlearray[8], titlearray[9], titlearray[10]
		MenuTitle = " "
	ENDIF

	format LocalMenuTitle as Mid(MenuTitle,1,maxCol)
	FOREVER
		WINDOW maxRow, maxCol, LocalMenuTitle
		IF len(trim(titlearray[1])) > 0
			FOR i = 1 to 10
				IF trim(titlearray[i]) <> ""
					DISPLAY Row, 1, trim(titlearray[i]){=63}//{=68}
					Row = Row + 1
				ELSE
					break
				ENDIF
			ENDFOR
			DISPLAY Row, 1, " "
			Row = Row + 1
		ENDIF
		WHILE Row < maxRow
			SeparatorPosition = INSTR(Column, _buffer, ";")
			IF SeparatorPosition = 0
				BREAK
			ENDIF
			SeparatorPosition = SeparatorPosition - Column
			tmpline = MID(_buffer, Column, SeparatorPosition)

			IF instr(1, tmpline, chr(10)) > 0
				FOREVER
					IF instr(tmpcolcount, tmpline, chr(10)) = 0
						DISPLAY Row, 1, mid(tmpline, tmpcolcount, len(tmpline) - tmpcolcount)
					ELSE
						DISPLAY Row, 1, mid(tmpline, tmpcolcount, instr(tmpcolcount, tmpline, chr(10) - tmpcolcount)
					ENDIF
					tmpcolcount = instr(tmpcolcount, tmpline, chr(10)) + 1
					Row = Row + 1

					IF tmpcolcount = 1
						break
					ENDIF
				ENDFOR
			ELSE
				DISPLAY Row, 1, tmpline
			ENDIF

			Column = Column + SeparatorPosition + 1
			Row = Row + 1
		ENDWHILE
		INPUTKEY localkey, InputAnswer, MenuTitle
		WINDOWCLOSE
		IF (localkey = @key_enter)
			Continua = "0"
			InputAnswer = "0"
			break
		ELSEIF (localkey = @key_clear)
			Continua = "0"
			InputAnswer = "1"
			break
		ELSEIF localkey = Key(1, 131323) // Voltar a tela anterior
			InputAnswer = "0"
			Continua = "1"
			Break
		else // (keypress_confirm <> @key_enter) and (keypress_confirm <> @key_clear) and (keypress_confirm <> Key(1, 131323))
			errormessage "E necessario digitar em SIM ou Nao!!!"
		ENDIF
	ENDFOR

ENDSUB
// ----------------------------------------------------------------------------

// ----------------------------------------------------------------------------
SUB sDisplay(REF _buffer)
	VAR maxRow : N5 = 12
	VAR maxCol : N5 = 65//70
	VAR bufferLength : N5
	VAR Row : N5 = 1
	VAR Column : N5 = 1

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sDisplay(", _buffer, ")"
	CALL WriteLogFile(gbl_LogText, cFalse)

	WINDOW maxRow, maxCol
	IF LEN(_buffer) > (maxRow * maxCol)
		bufferLength = (maxRow * maxCol)
	ELSE
		bufferLength = LEN(_buffer)
	ENDIF

	WHILE Column < bufferLength
		IF bufferLength - Column < maxCol
			maxCol = (bufferLength - Column) + 1
		ENDIF
		DISPLAY Row, 1, MID(_buffer, Column, maxCol)
		Column = Column + maxCol
		Row = Row + 1
	ENDWHILE
	WAITFORCONFIRM
	WINDOWCLOSE
ENDSUB

// ----------------------------------------------------------------------------

SUB sConectSiTEFServerWithOutExit(REF _result)
	VAR localSERVER_IP : A255
	VAR localIdTerminal : A20
	VAR Reservado : A6 = "000000"
	VAR Resultado : A6 = "000000"

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sConectSiTEFServerWithOutExit(", _result, ")"
	CALL WriteLogFile(gbl_LogText, cFalse)

	Prompt "Conectando com SITEF"
	
	DLLLOAD hDLLSitef, dLLSiTEF	

	IF hDLLSitef <> 0
		
		IF TRIM(IdTerminal) = "" // IdTerminal empty - must reload the interface
			//CALL sLogger(@line,"TEF - IdTerminal was empty - reloading the interface!", cfalse)
		ENDIF
		
		// start the process
		FORMAT localSERVER_IP AS "{", SERVER_IP, "}"
		FORMAT localIdTerminal AS IdTerminal, @wsid{>06}
		
		if ForceTEFLog = ctrue
			FORMAT gbl_LogText AS @line{06}, " ", _result, " ", localSERVER_IP, " ", IDLOJA, " ", localIdTerminal, " ", Reservado
			CALL WriteLogFile(gbl_LogText, ctrue)
		endif
		
		DLLCALL hDLLSitef, ConfiguraIntSiTefInterativoA(REF _result, REF localSERVER_IP, REF IDLOJA, REF localIdTerminal, REF Reservado)
		
		if ForceTEFLog = ctrue
			FORMAT gbl_LogText AS @line{06}, " ", _result, " ", localSERVER_IP, " ", IDLOJA, " ", localIdTerminal, " ", Reservado
			CALL WriteLogFile(gbl_LogText, ctrue)
		endif
		
		IF _result <> 0
			FORMAT gbl_LogText AS @line{06}, " ", "TEF - Error connecting server :", _result
			CALL WriteLogFile(gbl_LogText, ctrue)

			IF _result = 1
				ERRORMESSAGE "Endereco IP invalido ou nao resolvido"
			ELSEIF _result = 2
				ERRORMESSAGE "Codigo da loja invalido"
			ELSEIF _result = 3
				ERRORMESSAGE "Codigo de terminal invalido"
			ELSEIF _result = 6
				ERRORMESSAGE "Erro na inicializacao do Tcp/Ip"
			ELSEIF _result = 7
				ERRORMESSAGE "Falta de memoria"
			ELSEIF _result = 8
				ERRORMESSAGE "Nao encontrou a dll CliSiTef ou ela esta com problemas"
			ELSEIF _result = 10
				ERRORMESSAGE "O PinPad nao esta devidamente configurado no arquivo CliSiTef.ini"
			ELSE
				ERRORMESSAGE "Erro de Configuracao desconhecido: "//, _result
			ENDIF
			//CALL sFreeSiTEFDll()
		ENDIF
	ELSE
		//CALL sLogger(@line,"TEF - Error loading DLL", ctrue)
		ERRORMESSAGE "Nao foi possivel abrir a DLL do TEF!"
	ENDIF
ENDSUB

// ----------------------------------------------------------------------------
SUB sConectSiTEFServer(REF _result, ref __canceltype)
	VAR localSERVER_IP : A255
	VAR localIdTerminal : A20
	//VAR IDLOJA					: A20
	
	VAR Reservado : A6 = "000000"
	VAR Resultado : A6 = "000000"

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sConectSiTEFServer(", _result, __canceltype, ")"
	CALL WriteLogFile(gbl_LogText, cFalse)

	Prompt "Conectando com SITEF"
	
	DLLLOAD hDLLSitef, "CliSiTef32I.dll"

	IF hDLLSitef <> 0
				
		// start the process
		FORMAT localSERVER_IP AS "{", gbl_Conf_Tef_Server, "}"
		FORMAT localIdTerminal AS "MF", @wsid{>06}
				
		DLLCALL hDLLSitef, ConfiguraIntSiTefInterativoA(REF _result, REF localSERVER_IP, REF gbl_Conf_Tef_Loja , REF localIdTerminal, REF Reservado)
		
		FORMAT gbl_LogText AS @line{06}, " ", _result, " ", localSERVER_IP, " ", gbl_Conf_Tef_Loja, " ", localIdTerminal, " ", Reservado
		CALL WriteLogFile(gbl_LogText, ctrue)
		
		IF _result <> 0
			FORMAT gbl_LogText AS @line{06}, " ", "TEF - Error connecting server :", _result
			CALL WriteLogFile(gbl_LogText, ctrue)

			IF _result = 1
				ERRORMESSAGE "Endereco IP invalido ou nao resolvido"
			ELSEIF _result = 2
				ERRORMESSAGE "Codigo da loja invalido"
			ELSEIF _result = 3
				ERRORMESSAGE "Codigo de terminal invalido"
			ELSEIF _result = 6
				ERRORMESSAGE "Erro na inicializacao do Tcp/Ip"
			ELSEIF _result = 7
				ERRORMESSAGE "Falta de memoria"
			ELSEIF _result = 8
				ERRORMESSAGE "Nao encontrou a dll CliSiTef ou ela esta com problemas"
			ELSEIF _result = 10
				ERRORMESSAGE "O PinPad nao esta devidamente configurado no arquivo CliSiTef.ini"
			ELSE
				ERRORMESSAGE "Erro de Configuracao desconhecido: "//, _result
			ENDIF
			//CALL sFreeSiTEFDll()
		ENDIF
	ELSE
		//CALL sLogger(@line,"TEF - Error loading DLL", ctrue)
		ERRORMESSAGE "Nao foi possivel abrir a DLL do TEF!"
	ENDIF
ENDSUB

SUB sCleanVar(REF _buffer)

	VAR i : N9
	VAR StartPos :N09
	VAR lenghtbuffer :N09

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sCleanVar(", _buffer, ")"
	CALL WriteLogFile(gbl_LogText, cFalse)

	_buffer = trim(_buffer)

	i = instr(StartPos, _buffer, "{" )
	while i <> 0
		StartPos = 1
		lenghtbuffer = LEN(_buffer)
		if i <> 0
			if i = StartPos
				format _Buffer as Mid(_buffer, i+1, lenghtbuffer -1)
			elseif i = lenghtbuffer
				format _Buffer as Mid(_buffer, StartPos, lenghtbuffer -1)
			else
				format _Buffer as Mid(_buffer, StartPos, i-1), Mid(_buffer, i+1, lenghtbuffer - i)
			endif
		endif
		i = instr(StartPos, _buffer, "{" )
	endwhile

	i = instr(StartPos, _buffer, "}" )
	while i <> 0
		StartPos = 1
		lenghtbuffer = LEN(_buffer)
		if i <> 0
			if i = StartPos
				format _Buffer as Mid(_buffer, i+1, lenghtbuffer -1)
			elseif i = lenghtbuffer
				format _Buffer as Mid(_buffer, StartPos, lenghtbuffer -1)
			else
				format _Buffer as Mid(_buffer, StartPos, i-1), Mid(_buffer, i+1, lenghtbuffer - i)
			endif
		endif
		i = instr(StartPos, _buffer, "}" )
	endwhile

ENDSUB
// ----------------------------------------------------------------------------
// this functions evaluate the error generated in Resultado variable
SUB sEvaluateResultError(REF _result)

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sEvaluateResultError(", _result, ")"
	CALL WriteLogFile(gbl_LogText, cFalse)

	IF _result = "000000"
		ERRORMESSAGE "Negada pelo autorizador"
		FORMAT gbl_LogText AS @line{06}, " ", "TEF - ERRO: Negada pelo autorizador"
	ELSEIF _result = "-00001"
		ERRORMESSAGE "Modulo nao inicializado"
		FORMAT gbl_LogText AS @line{06}, " ", "TEF - ERRO: Modulo nao inicializado ret: -1"
	ELSEIF _result = "-00002"
		ERRORMESSAGE "Operacao cancelada pelo operador"
		FORMAT gbl_LogText AS @line{06}, " ", "TEF - ERRO: Operacao cancelada pelo operador ret: -2"
	ELSEIF _result = "-00003"
		ERRORMESSAGE "Fornecida uma modalidade invalida"
		FORMAT gbl_LogText AS @line{06}, " ", "TEF - ERRO: FORnecida uma modalidade invalida ret: -3"
	ELSEIF _result = "-00004"
		ERRORMESSAGE "Falta de memoria para executar a funcao"
		FORMAT gbl_LogText AS @line{06}, " ", "TEF - ERRO: Falta de memoria para executar a funcao ret: -4"
	ELSEIF _result = "-00005"
		ERRORMESSAGE "Sem comunicacao com o sitef"
		FORMAT gbl_LogText AS @line{06}, " ", "TEF - ERRO: Sem comunicacao com o sitef ret: -5"
	ELSE
		FORMAT gbl_LogText AS @line{06}, " ", "TEF - ERRO: Erro desconhecido: ", _result
	ENDIF
	CALL WriteLogFile(gbl_LogText, ctrue)
ENDSUB
// ----------------------------------------------------------------------------
// this function prepare the buffer to be sent to DLL; it MUST receive a string w/ 20000 characters!
SUB sFillBuffer(REF _buffer)
	VAR tmp : A20000

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sFillBuffer(", _buffer, ")"
	CALL WriteLogFile(gbl_LogText, cFalse)

	SETSTRING tmp , " ", (varsize(_buffer) - LEN(_buffer))
	FORMAT _buffer AS _buffer, tmp
ENDSUB
// ----------------------------------------------------------------------------
// this function prepare the buffer to be sent to DLL the CORRECT CMC-7 data (filled w/ 0's at left)
SUB sFillCMC7Buffer(REF _buffer, VAR num :N5)
	VAR tmp : A20
	VAR tmp2 : A200

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sFillCMC7Buffer(", _buffer, ", ", num, ")"
	CALL WriteLogFile(gbl_LogText, cFalse)

	FORMAT tmp2 AS TRIM(_buffer)
	SETSTRING tmp , "0", (num - LEN(_buffer))
	FORMAT _buffer AS tmp, tmp2
ENDSUB
// ----------------------------------------------------------------------------
// this function show the message send by TEF server to client
SUB sShowClientTEFMsg (REF _buffer)
	VAR tmp : A50
	VAR tmp2 : A50
	VAR lastspace : N5 = 0
	VAR lastvalidspace : N5
	VAR Resultado : A6
	VAR TMPVersion : A20
	VAR VersionPOS : $3

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sShowClientTEFMsg(", _buffer, ")"
	CALL WriteLogFile(gbl_LogText, cFalse)

	IF TRIM(_buffer) <> ""
		call sCleanVar(_buffer)
		FORMAT TMPVersion AS @version
		FORMAT TMPVersion AS MID(TMPVersion,1,3)
		VersionPOS = TMPVersion
		IF VersionPOS >= 4.10
			@sustaincustomtext = 1
			DisplayRearArea TRIM(Mid(_buffer,1,40))
		ENDIF
	ENDIF
ENDSUB
// ----------------------------------------------------------------------------
// this function put the default messagem in pinpad and free the sitef dll
SUB sFreeSiTEFDll()

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sFreeSiTEFDll()"
	CALL WriteLogFile(gbl_LogText, cFalse)

	//CALL sLogger(@line,"TEF - FreeSiTEFDll()", cfalse)
	DLLFREE hDLLSitef
ENDSUB
// ----------------------------------------------------------------------------
// this function finish the SiTEF transaction commiting IF ok and rollbacking IF something is wrong
SUB sFinishSiTEF(VAR transaction_ok :N01, REF NumeroCuponFiscal, VAR DataFiscal__ : A8, VAR Horario__ : A6)
	VAR DataFiscal : A8
	VAR Horario : A6
	VAR tmp1	:a1
	VAR tmp2	:a1
	VAR tmp3	:a1
	VAR tmp4	:a1
	VAR tmp5	:a1
	VAR tmp6	:a1
	VAR tmp7	:a1
	

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sFinishSiTEF(", transaction_ok , ", ", NumeroCuponFiscal, ", ", DataFiscal__, ", ", Horario__ , ")"
	CALL WriteLogFile(gbl_LogText, cFalse)

	if DataFiscal__ <> ""
		FORMAT DataFiscal AS DataFiscal__
	else
		FORMAT DataFiscal AS "20", @year{>02}, @month{>02}, @day{>02}
	endif
	if Horario__ <> ""
		FORMAT Horario AS Horario__
	else
		FORMAT Horario AS @hour{>02}, @minute{>02}, @second{>02}
	endif	
		
	if(hDLLSitef = 0)
		DLLLOAD hDLLSitef, dLLSiTEF	
	endif
				
	FORMAT gbl_LogText AS @line{06}, " ", "TEF - Confirmando TEF FinalizaTransacaoSiTefInterativoA(1,", NumeroCuponFiscal, ",", DataFiscal, ",", Horario, ")"
	CALL WriteLogFile(gbl_LogText, ctrue)
	if(transaction_ok = 1)			
		DLLCALL hDLLSitef, FinalizaTransacaoSiTefInterativoA("1", REF NumeroCuponFiscal, REF DataFiscal, REF Horario)
	else			
		DLLCALL hDLLSitef, FinalizaTransacaoSiTefInterativoA("0", REF NumeroCuponFiscal, REF DataFiscal, REF Horario)
	endif
		
	cleararray TEFVoucherBuffer1
	TEFVoucher1LineCnt = 0
	cleararray TEFVoucherBuffer2
	TEFVoucher2LineCnt = 0
	//call sWriteTefTransFile(TEFAUTHORIZATIONCNT, tmp1, tmp2, tmp3, tmp4, tmp5, tmp6, tmp7)
	//CALL sFreeSiTEFDll()
	//CALL sLogger(@line,"sFinishSiTEF() - is over!", cfalse)
	
	//call ExitSIM(0,"")
	
ENDSUB
// ----------------------------------------------------------------------------
SUB sDisplayPageable(REF _Header, REF _ListSize, REF _ListDetail[], ref __canceltype)
	VAR KeyPressed : Key
	VAR DataSelection : N5
	VAR PagesCountTtl : N3
	VAR PagesCount : N3
	VAR RowCount : N3
	VAR WinLen : N2
	VAR ClearLine : A78
	VAR PageSize : N3

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sDisplayPageable(", _Header, ", ", _ListSize, ", _ListDetail[], ", __canceltype, ")"
	CALL WriteLogFile(gbl_LogText, cFalse)

	SETSTRING ClearLine, " ", gbl_WndColSize[@wstype]
	WinLen = gbl_WndColSize[@wstype]

	Continua = "0"
	IF (@wstype = HHT)
		PageSize = 4
	ELSEIF (@wstype = WS4)
		PageSize = 12
	ELSEIF (@wstype = PCWS)
		PageSize = 12
	ELSE
		ERRORMESSAGE "Terminal Desconhecido"
		CALL ExitSim(__canceltype)
	ENDIF

	PagesCountTtl = (_ListSize/PageSize)
	IF (PagesCountTtl * PageSize) <> _ListSize
		PagesCountTtl = PagesCountTtl + 1
	ENDIF

	PagesCount = PagesCount + 1

	WINDOW PageSize, WinLen, _Header
	FOR RowCount = 1 to PageSize
		IF PagesCount <= PagesCountTtl
			DISPLAY RowCount, 1, ClearLine
			DISPLAY RowCount, 1, MID(_ListDetail[((PagesCount - 1) * PageSize) + RowCount], 1, WinLen)
		ENDIF
	ENDFOR

	FOREVER
		INPUTKEY KeyPressed, DataSelection, "Selecione ?"
		IF KeyPressed = @key_enter
			Continua = "0"
			RETURN
		ELSEIF KeyPressed = @key_clear
			Continua = "0"
			RETURN
		ELSEIF KeyPressed = KEY(1, 131081) //Page Up
			IF PagesCount > 1
				PagesCount = PagesCount - 1
			ENDIF
			FOR RowCount = 1 TO PageSize
				IF (((PagesCount - 1)*PageSize) + RowCount) <= _ListSize
					DISPLAY RowCount, 1, ClearLine
					DISPLAY RowCount, 1, MID(_ListDetail[((PagesCount - 1) * PageSize) + RowCount], 1, WinLen - 1)
				ELSE
					DISPLAY RowCount, 1, ClearLine
					DISPLAY RowCount, 1, MID(ClearLine, 1, WinLen)// - 1)
				ENDIF
			ENDFOR
		ELSEIF KeyPressed = Key(1, 131075) // Page Down
			IF PagesCount < PagesCountTtl
				PagesCount = PagesCount + 1
			ENDIF
			FOR RowCount = 1 TO PageSize
				IF (((PagesCount - 1) * PageSize) + RowCount) <= _ListSize
					DISPLAY RowCount, 1, ClearLine
					DISPLAY RowCount,1, MID(_ListDetail[((PagesCount - 1) * PageSize) + RowCount], 1, WinLen - 1)
				ELSE
					DISPLAY RowCount, 1, ClearLine
					DISPLAY RowCount, 1, MID(ClearLine, 1, WinLen)// - 1)
				ENDIF
			ENDFOR
		ELSEIF KeyPressed = Key(1, 131323) // Voltar a tela anterior
			Continua = "1"
			Break
		ENDIF
	ENDFOR
ENDSUB

// =====================================================================================
SUB sTEFVoucherVerify(REF NumeroCuponFiscal, REF Resultado)

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sTEFVoucherVerify(", NumeroCuponFiscal, ", ", Resultado, ")"
	CALL WriteLogFile(gbl_LogText, cFalse)
	// todo
	//	IF ISTEFTRANSACTION = cTrue
	CALL sFCRCMDStatus(Resultado)
	IF Resultado <> 0
		//CALL sLogger(@line,"TEF - Finalizado TEF com problemas", cfalse)
	ELSE
		CALL sFinishSiTEF(cTrue, NumeroCuponFiscal, "", "")
		//CALL sLogger(@line,"TEF - Finalizado TEF sem problemas", cfalse)
	ENDIF
	//	ENDIF
ENDSUB

SUB sYesOrNo(REF keypress_confirm, VAR string :A255)
	VAR data : A255
	VAR ask : A255

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sYesOrNo(", keypress_confirm, ", ", string , ")"
	CALL WriteLogFile(gbl_LogText, cFalse)

	FORMAT ask AS TRIM(string), "? (SIM/NAO)"
	FOREVER
		PROMPT TRIM(string)
		WINDOW 4, gbl_WndColSize[@wstype], "Responda"//TRIM(string)
		DISPLAY 1, 2, ask
		INPUTKEY keypress_confirm, data, ask
		IF (keypress_confirm <> @key_enter) and (keypress_confirm <> @key_clear)
			errormessage "E necessario digitar em SIM ou Nao!!!"
		ELSE
			break
		ENDIF
	ENDFOR
	WINDOWCLOSE
ENDSUB
// =====================================================
SUB sTEFErrorWHILEPrinting(REF NumeroCuponFiscal, ref _CancelType)
	VAR keypress_confirm : KEY
	VAR locStatusCMD : N1

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sTEFErrorWHILEPrinting(", NumeroCuponFiscal, ", ", _CancelType, ")"
	CALL WriteLogFile(gbl_LogText, cFalse)

	FOREVER
		CALL sYesOrNo(keypress_confirm, "Impressora nao responde. Deseja imprimir novamente")
		IF keypress_confirm = @key_enter // SIM
			call sVerify_Open_Doc_TEF(locStatusCMD)
			IF locStatusCMD <> 0				
			ELSE
				CALL sPrintTEFDoc(locStatusCMD, cfalse)
				if locStatusCMD = 0
					CALL sFinishSiTEF(cTrue, NumeroCuponFiscal, "", "")					
				endif
				IF locStatusCMD = 0 or locStatusCMD = -5
					break
				ENDIF
			ENDIF
		ELSEIF keypress_confirm = @key_clear

			prompt "Aguarde Cancelando TEF..."
			gbl_reprintALL = 1			
			CALL sFinishSiTEF(cFalse, NumeroCuponFiscal, "", "")			
			CALL ExitSim(1, "Transacao TEF Encerrada")
			locStatusCMD = -5
			IF locStatusCMD = 0 or locStatusCMD = -5
				break
			ENDIF
		ENDIF
	ENDFOR
ENDSUB

SUB PrintTEF_NaoFiscal(ref _status)
//Print TEF DOC (VAR globais)
	VAR countLines				:N9
	VAR tmpLines					:A60
		
	FORMAT gbl_LogText AS @line{06}, " ", "SUB - PrintTEF_NaoFiscal"
	CALL WriteLogFile(gbl_LogText, cFalse)
	
	Prompt "Imprimindo TEF"
	
	msleep(2000)
	
	call LoadDLLEpsonImpressao(_status)
		
	for countLines = 1 to TEFVoucher1LineCnt
		format tmpLines as "<c><b>", TEFVoucherBuffer1[countLines],"</b></c>", chr(10)
		dllcall hDLLEpson, ImprimeTextoTag(tmpLines)
	endfor	
	
	dllcall hDLLEpson, AcionaGuilhotina(0)
	
	for countLines = 1 to TEFVoucher2LineCnt
		format tmpLines as "<c><b>", TEFVoucherBuffer2[countLines],"</b></c>", chr(10)
		dllcall hDLLEpson, ImprimeTexto(tmpLines)
	endfor
	
	dllcall hDLLEpson, AcionaGuilhotina(1)
	
endsub

SUB sLoadCFG(REF _TMPValues1, REF _TMPValues2)
	VAR TMPFoundParameter : N1
	VAR stmp : A1024
	VAR stmp2 : A1024
	VAR stmp3 : A1024

	FORMAT gbl_LogText AS @line{06}, " ", "SUB - sLoadCFG ", _TMPValues1, " | ", _TMPValues2
	CALL WriteLogFile(gbl_LogText, cFalse)

	UPPERCASE _TMPValues1
	
	IF TRIM(_TMPValues1) = ""
		TMPFoundParameter = cFalse
	
	ELSEIF TRIM(_TMPValues1) = @wsid		
		TMPFoundParameter = cTrue
		gbl_Conf_IP_Print = TRIM(_TMPValues2)
	
	ELSEIF TRIM(_TMPValues1) = "IP_SERVER"
		TMPFoundParameter = cTrue
		gbl_Conf_IP_Send = TRIM(_TMPValues2)
			
	ELSEIF TRIM(_TMPValues1) = "PORTA"
		TMPFoundParameter = cTrue
		gbl_Conf_Porta_Send = TRIM(_TMPValues2)
	
	ELSEIF TRIM(_TMPValues1) = "IMPRIMIRDANFE"
		TMPFoundParameter = cTrue
		gbl_Conf_ImprimirDanfe = TRIM(_TMPValues2)
		
	ELSEIF TRIM(_TMPValues1) = "TEFSTART"
		TMPFoundParameter = cTrue
		gbl_Conf_TefStart = TRIM(_TMPValues2)
		
	ELSEIF TRIM(_TMPValues1) = "TEFEND"
		TMPFoundParameter = cTrue
		gbl_Conf_TefEnd = TRIM(_TMPValues2)	
	
	ELSEIF TRIM(_TMPValues1) = "TEFSTARTDEB"
		TMPFoundParameter = cTrue
		gbl_Conf_TefStartDEB = TRIM(_TMPValues2)
		
	ELSEIF TRIM(_TMPValues1) = "TEFENDDEB"
		TMPFoundParameter = cTrue
		gbl_Conf_TefEndDEB = TRIM(_TMPValues2)
		
	ELSEIF TRIM(_TMPValues1) = "TEFSTARTCRED"
		TMPFoundParameter = cTrue
		gbl_Conf_TefStartCRED = TRIM(_TMPValues2)
		
	ELSEIF TRIM(_TMPValues1) = "TEFENDCRED"
		TMPFoundParameter = cTrue
		gbl_Conf_TefEndCRED = TRIM(_TMPValues2)	
	
	ELSEIF TRIM(_TMPValues1) = "ENABLETEF"
		TMPFoundParameter = cTrue
		gbl_Conf_Tef_Enable = TRIM(_TMPValues2)
	
	ELSEIF TRIM(_TMPValues1) = "SERVER_IP"
		TMPFoundParameter = cTrue
		gbl_Conf_Tef_Server = TRIM(_TMPValues2)
		
	ELSEIF TRIM(_TMPValues1) = "IDLOJA"
		TMPFoundParameter = cTrue
		gbl_Conf_Tef_Loja = TRIM(_TMPValues2)
		
	ELSEIF TRIM(_TMPValues1) = "Linha1"
		TMPFoundParameter = cTrue
		gbl_Linha1 = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "Linha2"
		TMPFoundParameter = cTrue
		gbl_Linha2  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "Linha3"
		TMPFoundParameter = cTrue
		gbl_Linha3  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "Linha4"
		TMPFoundParameter = cTrue
		gbl_Linha4  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "Linha5"
		TMPFoundParameter = cTrue
		gbl_Linha5  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "Linha6"
		TMPFoundParameter = cTrue
		gbl_Linha6  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "Linha7"
		TMPFoundParameter = cTrue
		gbl_Linha7  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "Linha8"
		TMPFoundParameter = cTrue
		gbl_Linha8  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "Linha9"
		TMPFoundParameter = cTrue
		gbl_Linha9  = TRIM(_TMPValues2)			
		
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria  = TRIM(_TMPValues2)	
	
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA51"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria51  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA52"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria52  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA53"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria53  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA54"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria54  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA55"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria55  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA56"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria56  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA57"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria57  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA58"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria58  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA59"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria59  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA60"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria60  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA61"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria61  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA62"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria62  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA63"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria63  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA64"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria64  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA65"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria65  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA66"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria66  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA67"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria67  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA68"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria68  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA69"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria69  = TRIM(_TMPValues2)
	ELSEIF TRIM(_TMPValues1) = "PAGAMENTOHOTELARIA70"
		TMPFoundParameter = cTrue
		gbl_Conf_PagHotelaria70  = TRIM(_TMPValues2)	
	ELSEIF TRIM(_TMPValues1) = "GBL_CONF_DADOSPAGANTE"
		TMPFoundParameter = cTrue
		gbl_Conf_DadosPagante  = TRIM(_TMPValues2)		
	ELSEIF TRIM(_TMPValues1) = "SAT_NFCE"
		TMPFoundParameter = cTrue
		gbl_Conf_SAT_NFCE  = TRIM(_TMPValues2)
	
	ENDIF
	
	IF TMPFoundParameter = cTRUE
		FORMAT gbl_LogText AS @line, " ", "GLOBAL - Parametro ", TRIM(_TMPValues1), " = ", _TMPValues2
		CALL WriteLogFile(gbl_LogText, cfalse)
	ENDIF
ENDSUB

SUB sCheckMicrosCode(ref varInput )
		VAR tmp         : A255
		VAR itmp        : n5 = 0
		VAR itmp1       : n5 = 0
		VAR retries     : n5 = 0
		VAR keypress_confirm    : key
		
		varInput = @tremp

ENDSUB

SUB SaveCheckInfo(var msg : A100)
	
	var infoLines[18]				:A100
	var tmpCount						:N9
	var tmpLines						:N9
	
	tmpLines = 1
	
	//Lendo dados de hotelaria
		for tmpCount = 1 to @numdtlt		
			if((@dtl_type[tmpCount] = "I")and(len(trim(@dtl_name[tmpCount])) > 1))
				format infoLines[tmpLines] as @dtl_name[tmpCount]
				tmpLines = tmpLines +1
			endif
		endfor
		
		clearchkinfo
		
		for tmpCount = 1 to (tmpLines -1)
			savechkinfo infoLines[tmpCount]
		endfor
			savechkinfo msg	

ENDSUB

SUB sNewAliqFunction(ref FinalMSGAliq)	
	VAR CountItems : N07
	
	var objNum :n9
	var name :A100
	var ncm :A10
	
	var aliqF :A10 //federal
	var aliqE :A10 //estadual
	var aliqM :A10 //municipal
	
	var currentLine :n8
	var handleFileToRead :n100
	var handleFileToWrite :n100
	var FileToRead :A100 = "Aliquotas.txt"
	var NamesArray[1000] :A100
	var ItensArray[1000] :n8
	
	var AliqsArrayF[1000] :A10
	var AliqsArrayE[1000] :A10
	var AliqsArrayM[1000] :A10
	
	var Ttl_Array[1000] :$12
	
	var tmpValue:A100
	var tmpValueF :A50
	var tmpValueE :A50
	var tmpValueM :A50
	
	var index :n8
	var tmpAmount :$12
	var ttlAmount :$12
	
	var finalAmount	:$12
	
	var finalAmountF :$12
	var finalAmountE :$12
	var finalAmountM :$12
	
	var aliqAproxF :$12
	var aliqAproxE :$12
	var aliqAproxM :$12
	
	var tmpTxt :A100
	
	//FORMAT TMPLOG AS @line{06}, " ", "SUB - sNewAliqFunction()"
	//CALL sWriteLogFile(TMPLOG, cfalse)
				
	index = 0

	FOR CountItems = 1 to @NUMDTLT
		IF (@dtl_type[CountItems]="M") AND (@dtl_ttl[CountItems] > 0)
			index = index +1
			NamesArray[index] = @dtl_name[CountItems]			
			ItensArray[index] = @dtl_object[CountItems]			
			Ttl_Array[index] = @dtl_ttl[CountItems]
			ttlAmount = ttlAmount + @Dtl_Ttl[CountItems]
		ENDIF		
	ENDFOR	
	CountItems = index
	
	
		
		for index = 1 to CountItems
			
			FOPEN handleFileToRead, fileToRead, read
	
			if handleFileToRead <> 0				
		
				WHILE NOT FEOF(handleFileToRead)			
					FREADLN handleFileToRead, tmpValue
					split tmpValue, ";", objNum, name, ncm, aliqF, aliqE, aliqM
														
					if objNum = ItensArray[index]
						AliqsArrayF[index] = aliqF
						AliqsArrayE[index] = aliqE
						AliqsArrayM[index] = aliqM
						break
					endif			
					
				ENDWHILE		
				FCLOSE handleFileToRead
			else
				//Tratar erro?
			endif
			
			
			
		endfor
		
		//FCLOSE handleFileToRead
		
		for index = 1 to CountItems
						
			tmpValueF = (Ttl_Array[index] * AliqsArrayF[index]) / 100									
			tmpValueE = (Ttl_Array[index] * AliqsArrayE[index]) / 100
			tmpValueM = (Ttl_Array[index] * AliqsArrayM[index]) / 100
			
			finalAmountF = finalAmountF + tmpValueF 
			finalAmountE = finalAmountE + tmpValueE 
			finalAmountM = finalAmountM + tmpValueM
			
			finalAmount = finalAmount + tmpValueF + tmpValueE + tmpValueM
		
		endfor
				
		aliqAproxF = ((finalAmountF / ttlAmount) * 100)				
		aliqAproxE = ((finalAmountE / ttlAmount) * 100)
		aliqAproxM = ((finalAmountM / ttlAmount) * 100)		
		
		//format FinalMSGAliq[1] as chr(10),chr(10),"Discriminacao de Impostos",chr(10)
		//format FinalMSGAliq[2] as "Impostos Federais R$ ", finalAmountF, "(", aliqAproxF, "%)", chr(10)
		//format FinalMSGAliq[3] as "Impostos Estaduais R$ ", finalAmountE, "(", aliqAproxE, "%)", chr(10)
		//format FinalMSGAliq[4] as "Impostos Municipais R$ ", finalAmountM, "(", aliqAproxM, "%)", chr(10)
		
		format FinalMSGAliq as "Imp F: ", finalAmountF, "(", aliqAproxF, "%)", " E: ", finalAmountE,"(", aliqAproxE, "%)", " IBPT"
		
		//format FinalMSGAliq[5] as "Val Aprox Impostos: ", finalAmount, "    Fonte: IBPT"
				
	endif
ENDSUB