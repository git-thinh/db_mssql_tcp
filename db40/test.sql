use [demo];
go
declare	@ok int, @str_result nvarchar(max), @text nvarchar(36) = newid();

--==============================================================================
--[ NOTIFY ]

--exec @ok = dbo.notify___user @ref_id = 1, @user_id = 2, @str_message = @text, @str_result = @str_result output; print @ok; print @str_result;
--exec @ok = dbo.notify___users @ref_id = 1, @user_ids = '1,2', @str_message = @text, @str_result = @str_result output; print @ok; print @str_result;
--exec @ok = dbo.notify___all @ref_id = 1, @str_message = @text, @str_result = @str_result output; print @ok; print @str_result;

-- delete from mobile.pol_notify; delete from mobile.pol_notify_state
-- select * from mobile.pol_notify; select * from mobile.pol_notify_state

--==============================================================================
--[ VALID ]

--set @ok = dbo.valid___number('123'); print @ok;
--set @ok = dbo.valid___number_yyyyMMdd('19991231'); print @ok;
--set @ok = dbo.valid___number_HHmmss('121160'); print @ok;

--[ RENDER ]

--set @str_result = dbo.render___simple('Hello, {{Name}}!!!',N'{"Name1:"Mr Thinh"}',1); print @str_result;

--[ TCP ]

--set @ok = dbo.tcp___send('127.0.0.1',3456,newid()); print @ok;
