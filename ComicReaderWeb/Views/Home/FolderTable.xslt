<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="html" indent="yes"/>
  <xsl:param name="requestUrl"/>
  <xsl:param name="apiLocation"/>

  <!--Content table generation -->
  
  <xsl:template match="/">
    <table>    
      <th>Comics available</th>
      <xsl:apply-templates/>
    </table>
  </xsl:template>

  <xsl:template match="file">
    <tr>
      <td>
        <img alt="thumbnail">
          <xsl:attribute name="src">
            <xsl:value-of select="$apiLocation"/>
            <xsl:text>?file=</xsl:text>
            <xsl:value-of select="@path"/>
            <xsl:text>&amp;page=0&amp;size=50</xsl:text>
          </xsl:attribute>
        </img>
      </td>
      <td>
        <a>
          <xsl:attribute name="href">
            <xsl:value-of select="$requestUrl"/>
            <!--xsl:text>/Comic/Read/?file=</xsl:text-->
            <xsl:text>/Comic/Read</xsl:text>
            <xsl:value-of select="@path"/>
          </xsl:attribute>
          <xsl:value-of select="@name"/>
        </a>
      </td>
    </tr>
  </xsl:template>

</xsl:stylesheet>
