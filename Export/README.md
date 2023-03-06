#Apache IoTDB 数据导出工具 v(1.0.23)

```
--host=127.0.0.1
--port=6667
--user=root
--pwd=admin#123
--database=kylin
--zip=true|false
--start=2022-11-01
--end=2022-11-10 
--point=
```

调用示例如上, 去掉'--'将参数写入 `export.ini` 即可直接执行

### export.ini
``` ini
host=192.168.0.11
port=6667
user=root
pwd=admin#123
start=2022-11-09
end=2022-11-12
zip=true
point=KD_ID_U2_SPL_PIN_KB,KA_OA_PL_TARG_SC_CO
```