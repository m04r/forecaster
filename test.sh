#!/bin/bash

touch test-output
truncate -s 0 test-output

echo 'Seattle' >> test-output
http --verify=no GET https://localhost:7232/Forecast?latitude=47.607999\&longitude=-122.335 >> test-output
echo -e '\r\nDallas' >> test-output
http --verify=no GET https://localhost:7232/Forecast?latitude=32.779167\&longitude=-96.808891 >> test-output
echo -e '\r\nSan Francisco' >> test-output
http --verify=no GET https://localhost:7232/Forecast?latitude=37.773972\&longitude=-122.431297 >> test-output
echo -e '\r\nDenver' >> test-output
http --verify=no GET https://localhost:7232/Forecast?latitude=39.742043\&longitude=-104.991531 >> test-output
echo -e '\r\nChicago' >> test-output
http --verify=no GET https://localhost:7232/Forecast?latitude=41.881832\&longitude=-87.623177 >> test-output
echo >> test-output
