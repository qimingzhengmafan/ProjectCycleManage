/*
 Navicat Premium Dump SQL

 Source Server         : 阿里云
 Source Server Type    : MySQL
 Source Server Version : 80036 (8.0.36)
 Source Host           : rm-uf694p49ucaz7035xvo.mysql.rds.aliyuncs.com:3306
 Source Schema         : huadatest1

 Target Server Type    : MySQL
 Target Server Version : 80036 (8.0.36)
 File Encoding         : 65001

 Date: 06/10/2025 20:25:07
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;



-- ----------------------------
-- Records of documenttype
-- ----------------------------
INSERT INTO `documenttype` VALUES (101, 'OA申请单号');
INSERT INTO `documenttype` VALUES (102, '设备申请表');
INSERT INTO `documenttype` VALUES (103, '技术协议');
INSERT INTO `documenttype` VALUES (104, '设备方案 OR Boom清单');
INSERT INTO `documenttype` VALUES (105, '设备项目问题改善');
INSERT INTO `documenttype` VALUES (106, '设备验证记录');
INSERT INTO `documenttype` VALUES (107, '培训记录');
INSERT INTO `documenttype` VALUES (108, '说明书');
INSERT INTO `documenttype` VALUES (109, '维保文件');
INSERT INTO `documenttype` VALUES (110, 'WI');
INSERT INTO `documenttype` VALUES (111, '设备验收单');
INSERT INTO `documenttype` VALUES (112, '文件发放记录');
INSERT INTO `documenttype` VALUES (113, 'OA领用单号');


INSERT INTO `equipmenttype` VALUES (101, '非标外购');
INSERT INTO `equipmenttype` VALUES (102, '非标自制');
INSERT INTO `equipmenttype` VALUES (103, '标准外购');


INSERT INTO `peopletable` VALUES (101, '朱成绪');
INSERT INTO `peopletable` VALUES (102, '董鑫');
INSERT INTO `peopletable` VALUES (103, '裴涛');
INSERT INTO `peopletable` VALUES (104, '江琛');
INSERT INTO `peopletable` VALUES (105, '王嘉豪');
INSERT INTO `peopletable` VALUES (106, '张园园');
INSERT INTO `peopletable` VALUES (107, '严鑫');


INSERT INTO `projectphasestatus` VALUES (101, '未启动');
INSERT INTO `projectphasestatus` VALUES (102, '进行中');
INSERT INTO `projectphasestatus` VALUES (103, '暂停');
INSERT INTO `projectphasestatus` VALUES (104, '已完成');

-- ----------------------------
-- Records of projectstage
-- ----------------------------
INSERT INTO `projectstage` VALUES (101, '项目评审');
INSERT INTO `projectstage` VALUES (102, '设备采购');
INSERT INTO `projectstage` VALUES (103, '预验收');
INSERT INTO `projectstage` VALUES (104, '设备验收');
INSERT INTO `projectstage` VALUES (105, '完成');


-- ----------------------------
-- Records of typetable
-- ----------------------------
INSERT INTO `typetable` VALUES (101, '复制');
INSERT INTO `typetable` VALUES (102, '新增');
INSERT INTO `typetable` VALUES (103, '改善');

-- ----------------------------
-- Records of projects
-- ----------------------------
INSERT INTO `projects` VALUES (1, 2022, NULL, 'Hilti整机报警电压自动切换设备', 'HDGC2022001', 102, 102, 105, NULL, '25000', '25000', 100, 104, 101, NULL, 'HD-3101-4343', NULL , NULL , NULL ,NULL);


SET FOREIGN_KEY_CHECKS = 1;
