# Add project specific ProGuard rules here.
# You can control the set of applied configuration files using the
# proguardFiles setting in build.gradle.
#
# For more details, see
#   http://developer.android.com/guide/developing/tools/proguard.html

# If your project uses WebView with JS, uncomment the following
# and specify the fully qualified class name to the JavaScript interface
# class:
#-keepclassmembers class fqcn.of.javascript.interface.for.webview {
#   public *;
#}

# Uncomment this to preserve the line number information for
# debugging stack traces.
#-keepattributes SourceFile,LineNumberTable

# If you keep the line number information, uncomment this to
# hide the original source file name.
#-renamesourcefileattribute SourceFile

-keep class com.zh.pocket.** {*;}

# AnyThink SDK 核心保持规则
-keep class com.anythink.** { *; }
-keep interface com.anythink.** { *; }
-keep class com.topon.** { *; } # 如果使用了 TopOn 品牌名
-keep interface com.topon.** { *; }

# 保持枚举和内部类
-keepclassmembers enum com.anythink.** { *; }
-keepclassmembers class com.anythink.** { *; }

# 如果使用了特定的广告网络适配器，也需要保持对应包名
# 例如: -keep class com.anythink.adapter.** { *; }